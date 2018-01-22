$(document).ready(function () {

    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
    $("#StartDate").attr('type', 'text');
    $("#EndDate").attr('type', 'text');
    $('#NoOfDays').attr('readonly', 'readonly');
    $('#NoOfDays').css('background-color', '#D4D4D4');

    $("#ishalf").click(function () {              //Show Am/Pm option for half days
        var diff = parseFloat($('#NoOfDays').val());
        if ($(this).prop('checked') === true) {
            $("#amorpm").show();           
            diff = diff - 0.5;
            $(".morning").prop("checked", true);

        } 
        else {            
            diff = diff + 0.5;           
            $("#HalfDay").prop("checked", false);
            $("#amorpm").hide();
        }
        $('#NoOfDays').val(diff);
    });
    


    var select = function (dateStr) {                   //calculate the number of days

        var d1 = $('#StartDate').datepicker('getDate');
        var d2 = $('#EndDate').datepicker('getDate');
        var point = ".";

        var diff = 0;
        if (d1 && d2) {

            diff = Math.floor((d2.getTime() - d1.getTime()) / 86400000); // ms per day
            diff = parseFloat(diff) + 1;

            while (d1 <= d2) {
                if (d1.getDay() === 5) {
                    diff -= 0.5;
                }
                if (d1.getDay() === 6) { // if it is saturday
                    diff--;
                }
                if (d1.getDay() === 0) { // if it is sunday
                    diff--;
                }
                d1.setDate(d1.getDate() + 1);
            }

            $('#NoOfDays').val(diff);

        }
        else {
            $('#NoOfDays').val(0);
        }

        if (diff > 0)                  // Hide or show half day option only when there is a valid value in Number OF Days Field
        {
            $(".halfday").show();
        }
        else {
            $(".halfday").hide();
        }


    };

    $("#StartDate").datepicker({        // intialize the date fields with datepicker funtion (start function)
        minDate: 0,        
        onSelect: select,
        dateFormat: 'dd/mm/yy'
        
    });
    $('#EndDate').datepicker({ minDate: 0, onSelect: select, dateFormat: 'dd/mm/yy' }); // intialize the date fields with datepicker funtion (start function)
      
   

});