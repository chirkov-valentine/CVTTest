$(document).ready(function () {
    var events = [];
    var selectedEvent = null;
    var filterString = null;
    FetchEventAndRenderCalendar();
    function FetchEventAndRenderCalendar() {
        events = [];
        getFilterString()
        $.ajax({
            type: "GET",
            url: "/home/GetEvents"+filterString,
            success: function (data) {
                $.each(data, function (i, v) {
                    events.push({
                        eventID: v.EventId,
                        title: v.Title,
                        meetingPoint: v.MeetingPoint,
                        start: moment(v.StartDate),
                        end: v.EndDate != null ? moment(v.EndDate) : null,
                        eventTypeId: v.EventTypeId,
                        isFinished: v.IsFinished
                    });
                }),

                GenerateCalender(events);
            },
            error: function (error) {
                alert(error.responseText);
            }
        })
    }

    function GenerateCalender(events) {
        $('#calender').fullCalendar('destroy');
        $('#calender').fullCalendar({
            contentHeight: 400,
            defaultDate: new Date(),
            timeFormat: 'H(:mm)',
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,basicWeek,basicDay,agenda,listWeek'
            },
            eventLimit: true,
            eventColor: '#378006',
            events: events,
            eventClick: function (calEvent, jsEvent, view) {
                selectedEvent = calEvent;
                $('#myModal #eventTitle').text(calEvent.title);
                var $description = $('<div/>');
                $description.append($('<p/>').html('<b>Тип события: </b>' + getEventName(calEvent.eventTypeId)));
                $description.append($('<p/>').html('<b>Время начала:</b>' + calEvent.start.format("DD.MM.YYYY HH:mm")));             
                if (calEvent.end != null) {
                    $description.append($('<p/>').html('<b>Время окончания:</b>' + calEvent.end.format("DD.MM.YYYY HH:mm")));
                }
                if (calEvent.meetingPoint != null) {
                    $description.append($('<p/>').html('<b>Место:</b>' + calEvent.meetingPoint));
                }
                
                $('#myModal #pDetails').empty().html($description);

                $('#myModal').modal();
            },
            selectable: true,
            select: function (start, end) {
                selectedEvent = {
                    eventID: 0,
                    title: '',
                    meetingPoint: '',
                    start: start,
                    end: end,
                    isFinished: false,
                    eventTypeId: 1
                };
                openAddEditForm();
                $('#calendar').fullCalendar('unselect');
            },
            editable: true,
            eventDrop: function (event) {
                var data = {
                    EventId: event.eventID,
                    Title: event.title,
                    StartDate: event.start.format('DD.MM.YYYY HH:mm'),
                    EndDate: event.end != null ? event.end.format('DD.MM.YYYY HH:mm') : null,
                    MeetingPoint: event.meetingPoint,
                    EventTypeId: event.eventTypeId,
                    IsFinished: event.isFinished
                };
                SaveEvent(data);
            },
            // русификатор
            monthNames: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'οюнь', 'οюль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
            monthNamesShort: ['Янв.', 'Фев.', 'Март', 'Апр.', 'Май', 'Июнь', 'Июль', 'Авг.', 'Сент.', 'Окт.', 'Ноя.', 'Дек.'],
            dayNames: ["Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота"],
            dayNamesShort: ["ВС", "ПН", "ВТ", "СР", "ЧТ", "ПТ", "СБ"],
            buttonText: {
                
                today: "Сегодня",
                month: "Месяц",
                week: "Неделя",
                day: "День"
            }
        })
    }

    $('#btnEdit').click(function () {
        //Открыть модальную форму
        openAddEditForm();
    })
    $('#btnDelete').click(function () {
        if (selectedEvent != null && confirm('Вы уверены?')) {
            $.ajax({
                type: "POST",
                url: '/home/DeleteEvent',
                data: { 'eventID': selectedEvent.eventID },
                success: function (data) {
                    if (data.status) {
                        //Обновить
                        FetchEventAndRenderCalendar();
                        $('#myModal').modal('hide');
                    }
                },
                error: function () {
                    alert('Ошибка!');
                }
            })
        }
    })

    $('#dtp1,#dtp2').datetimepicker({
        locale: 'ru'
    });

    $('#ddEventType').change(function () {
        $('#divMeetingPoint').hide();
        $('#divEndDate').show();
        var selectedOption = $(this).val();
        if (selectedOption == 1)
            $('#divMeetingPoint').show();
        if (selectedOption == 3)
            $('#divEndDate').hide();
    });

    $("#chkMeeting, #chkBusiness, #chkJotting").change(function() {
        FetchEventAndRenderCalendar();
    });

    function openAddEditForm() {
        if (selectedEvent != null) {
            $('#hdEventID').val(selectedEvent.eventID);
            $('#txtTitle').val(selectedEvent.title);
            $('#txtStart').val(selectedEvent.start.format('DD.MM.YYYY HH:mm '));
            $('#chkIsFinished').prop("checked", selectedEvent.isFinished || false);
            $('#ddEventType').change();
            $('#txtEnd').val(selectedEvent.end != null ? selectedEvent.end.format('DD.MM.YYYY HH:mm ') : '');
            $('#txtMeetingPoint').val(selectedEvent.meetingPoint);
            $('#ddEventType').val(selectedEvent.eventTypeId);
        }
        $('#myModal').modal('hide');
        $('#myModalSave').modal();
    }

    $('#btnSave').click(function () {
        // Валидация
        if ($('#txtTitle').val().trim() == "") {
            alert('Subject required');
            return;
        }
        if ($('#txtStart').val().trim() == "") {
            alert('Start date required');
            return;
        }
        if ($('#chkIsFinished').is(':checked') == false && $('#txtEnd').val().trim() == "") {
            alert('End date required');
            return;
        }
        else {
            var startDate = moment($('#txtStart').val(), "DD/MM/YYYY HH:mm A").toDate();
            var endDate = moment($('#txtEnd').val(), "DD/MM/YYYY HH:mm A").toDate();
            if (startDate > endDate) {
                alert('Invalid end date');
                return;
            }
        }

        var data = {
            EventId: $('#hdEventID').val(),
            Title: $('#txtTitle').val().trim(),
            StartDate: $('#txtStart').val().trim(),
            EndDate: $('#chkIsFinished').is(':checked') ? null : $('#txtEnd').val().trim(),
            MeetingPoint: $('#txtMeetingPoint').val(),
            EventTypeId: $('#ddEventType').val(),
            IsFinished: $('#chkIsFinished').is(':checked')
        }
        SaveEvent(data);

    })

    function SaveEvent(data) {
        $.ajax({
            type: "POST",
            url: '/home/SaveEvent',
            data: data,
            success: function (data) {
                if (data.status) {
                    //Обновить
                    FetchEventAndRenderCalendar();
                    $('#myModalSave').modal('hide');
                }
            },
            error: function (error) {
                alert(error.responseText);
            }
        })
    }

    function getEventName(typeId) {
        if (typeId == '1')
            return 'Встреча';
        if (typeId == '2')
            return 'Дело';
        if (typeId == '3')
            return 'Памятка';
    }

    function getFilterString() {
        filterString = '?meeting=' + $('#chkMeeting').is(':checked') + '&business=' + $('#chkBusiness').is(':checked')
            + '&jotting=' + $('#chkJotting').is(':checked');
    }
})