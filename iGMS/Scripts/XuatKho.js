$('#idsaleorder').keyup(function () {
    var id = $('#idsaleorder').val().trim().substring(5)
    Show(id)
})

function Show(id) {
    $.ajax({
        url: '/detailwarehouse/Show2',
        type: 'get',
        data: {
            id
        },
        success: function (data) {
            $('span[name="K"]').empty()
            $('span[name="customer"]').empty()
            $('tbody[name="tbd"]').empty()
            var Stt=1
            if (data.code == 200) {
                $.each(data.a, function (k, v) {
                    $('span[name="K"]').append(v.K)
                    $('span[name="K"]').attr('id',v.Kid)
                    $('span[name="customer"]').append(v.customer)
                })
                $.each(data.c, function (k, v) {
                    let table = '<tr id="' + v.id + '" class="donhang">'
                    table += '<td style="background:red" id="result' + v.id + '">0</td>'
                    table += '<td>' + (Stt++) + '</td>'
                    table += '<td>' + v.id + '</td>'
                    table += '<td>' + v.name + '</td>'
                    table += '<td id="amountresult' + v.id + '">' + v.amount + '</td></tr>'
                    $('tbody[name="tbd"]').append(table)
                })
            }  else {
                alert(data.msg)
            }
        }
    })
}

function CompareReceipt(barcode) {
    var amounttext = $('#result' + barcode + '').text();
    var amountresulttext = $('#amountresult' + barcode + '').text();
    var amount = Number(amounttext)
    var amountresult = Number(amountresulttext)
    var GoodPucharseOder = $('.donhang').map(function () {
        return this.id;
    })
    for (var i = 0; i < GoodPucharseOder.length; i++) {
        if (GoodPucharseOder[i] == barcode) {

            amount += 1;
            if (amount > amountresult) {
                return;
            } else {
                $('#result' + barcode + '').text(amount)
            }
            if (amount == amountresult) {
                $('#result' + barcode + '').css('background', 'green')
            } else if (amount < amountresult) {
                $('#result' + barcode + '').css('background', '#ffa800')
            } else if (amount <= 0) {
                $('#result' + barcode + '').css('background', 'red')
            }

        }
    }
}

//------------



function Add() {
    var ids = $('.donhang').map(function () {
        return this.id
    }).get();
    var idsaleorder = $('#idsaleorder').val().trim().substring(5)
    var id = ""
    for (let i = 0; i < ids.length; i++) {
        id = ids[i]
    }
    var amount = $('#result' + id + '').text()
    var K = $('span[name="K"]').attr('id');
    $.ajax({
        url: '/detailwarehouse/Tru',
        type: 'post',
        data: {
            id, idsaleorder, amount,K
        },
        success: function (data) {
            if (data.code == 200) {

            }
            else if (data.code == 1) {
                alert(data.msg)
            } else {
                alert(data.msg)
            }
        }
    })
}

//-----------------------------
$(document).scannerDetection({
    timeBeforeScanTest: 200, // wait for the next character for upto 200ms
    startChar: [120],
    endChar: [13], // be sure the scan is complete if key 13 (enter) is detected
    avgTimeByChar: 40, // it's not a barcode if a character takes longer than 40ms
    ignoreIfFocusOn: 'input', // turn off scanner detection if an input has focus
    minLength: 1,
    onComplete: function (barcode, qty) {
        CompareReceipt(barcode)

    }, // main callback function
    scanButtonKeyCode: 116, // the hardware scan button acts as key 116 (F5)
    scanButtonLongPressThreshold: 5, // assume a long press if 5 or more events come in sequence
    onError: function (string) { alert('Error ' + string); }
});