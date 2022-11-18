//----------------------Scan::barcode--------------------

$('#id').keyup(function () {
    var id = $('#id').val().trim();
    if (id.length == 9) {
        Scan(id);
    }
})



function Scan(id) {
    $.ajax({
        url: '/login/Scan',
        type: 'get',
        data: {
            id
        },
        success: function (data) {
            if (data.code == 200) {
                $('#user').val(data.user);
                $('#pass').val(data.pass);

                window.location.href = data.Url;
                
            } else if (data.code == 300) {
                alert(data.msg)
            }
            else {
                
            }
        }
    })
}

//------------------------Login-----------------

function Login() {
    var user = $('#user').val().trim();
    var passs = $('#pass').val().trim();
    var pass = md5(passs)
    $.ajax({
        url: '/login/LoginiGMS',
        type: 'get',
        data: {
            user, pass
        },
        success: function (data) {
            if (data.code == 200) {
                
                window.location.href = data.Url;
              
            } else if (data.code == 300) {
                alert(data.msg)
            }
            else {
                alert(data.msg)
            }
        }
    })
}

//-----------------------------
$(document).scannerDetection({
    timeBeforeScanTest: 200, // wait for the next character for upto 200ms
    endChar: [13], // be sure the scan is complete if key 13 (enter) is detected
    avgTimeByChar: 40, // it's not a barcode if a character takes longer than 40ms
    ignoreIfFocusOn: 'input', // turn off scanner detection if an input has focus
    minLength:1,
    onComplete: function (barcode, qty) {
        Scan(barcode);
    }, // main callback function
    scanButtonKeyCode: 116, // the hardware scan button acts as key 116 (F5)
    scanButtonLongPressThreshold: 5, // assume a long press if 5 or more events come in sequence
    onError: function (string) { alert('Error ' + string); }
});
