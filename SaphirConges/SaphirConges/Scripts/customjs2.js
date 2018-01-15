




$(document).ready(function () {

    var d1;
    var d2;
    $("#StartDate").attr('type', 'text');
    $("#EndDate").attr('type', 'text');
    //$('#NoOfDays').attr('readonly', 'readonly');
    $('#NoOfDays').css('background-color', '#D4D4D4');
    
  
    var calendar = $.calendars.instance('islamic');
    $("#StartDate").calendarsPicker({
        
        onSelect: startDate,
        dateFormat: 'dd/mm/yyyy',
        calendar: calendar
    });
    $("#EndDate").calendarsPicker({
        
        onSelect: endDate,
        dateFormat: 'dd/mm/yyyy',
        calendar: calendar
    });
    $('#EndDate').calendarsPicker({ calendar: calendar, onSelect: startDate });
    $('#StartDate').calendarsPicker({ calendar: calendar, onSelect: endDate });
    function startDate(date) {
       
       d1 = date;



    }
    function endDate(date) {
        d2 = date;
       
        d1 = Date.parse(d1);
        d2 = Date.parse(d2);

        var diff = 0;
        if (d1 && d2) {
            if (d1 === d2) {
                diff = parseFloat(0.5);
            }
            else
                diff = Math.floor((d2 - d1) / 86400000); // ms per day
        }
        diff = parseFloat(diff);
        $('#NoOfDays').val(diff);





    }




});
