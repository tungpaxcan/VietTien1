//-------------------chọn nơi xuất hàng--------------
$('input[type="radio"]').click(function () {
    var radio = document.getElementsByName('radio');
    for (let i = 0; i < radio.length; i++) {
        if (radio[i].checked) {
            switch (radio[i].value) {
                case "CH": $('#warehouse').attr("disabled", true)
                    $('#store').attr("disabled", false);
                    $('#warehouse').val(-1)
                    break;
                case "KH": $('#store').attr("disabled", true);
                    $('#warehouse').attr("disabled", false);
                    $('#store').val(-1)
                    break;
            }
        }
    }
})

//-------------------tim hang hoa
$('#seachidgood').keypress(function(event){
    if (event.which == 13) {
        var id = $('#seachidgood').val().trim();
        Good(id);
    }
})

function Good(id) {
    $.ajax({
        url: '/saleorder/Good',
        type: 'get',
        data: {
            id
        },
        success: function (data) {
            if (data.code == 200) {
                var Stt = 1;
                $.each(data.c, function (k, v) {
                    let table = '<tr id="' + v.id + '" role="row" class="odd IDGOOD">';
                    table += '<td class="datatable-cell-sorted datatable-cell-center datatable-cell datatable-cell-check" data-field="RecordID" aria-label="2"><span style="width: 30px;"><label class="checkbox checkbox-single kt-checkbox--solid"><input id="' + v.id + 'abc" onclick="Sumprice();" type="checkbox" value="' + v.id + '" name="change">&nbsp;<span></span></label></span></td>'
                    table += '<td>' + (Stt++) + '</td>'
                    table += '<td >' + v.id + '</td>'
                    table += '<td>' + v.name + '</td>'
                    table += '<td>' + v.unit + '</td>'
                    table += '<td><input type="number" value="0" id="amount' + v.id + '" /></td>'
                    table += '<td><input type="text" value="' + v.price + '" id="price' + v.id + '" /></td>'
                    table += '<td><input type="number" value="' + v.discount + '" id="discount' + v.id + '" placeholder="%" /></td>'
                    table += '<td id="pricediscount' + v.id + '"></td>'
                    table += '<td><input type="number" value="' + v.tax + '" id="tax' + v.id + '" /></td>'
                    table += '<td id="pricetax' + v.id + '"></td>'
                    table += '<td id="sumpricegoods' + v.id + '"></td>'
                    table += '</tr>';
                    $('#tbd').append(table);
                   $('#amount' + v.id + '').val()
                })
                IdGood(id)
            }
        }
    })
}
//-----------tim va tang ma đã có-----------
function IdGood(id) {
    var amount = $('#amount' + id + '').val().trim()
    var IDGOOOD = $('.IDGOOD').map(function () {
        return this.id;
    })
    for (let i = 0; i < IDGOOOD.length; i++) {
        var ids = IDGOOOD[i]
        if (ids == id) {
            $('#amount' + id + '').val(Number(amount) + 1)
        }
    }

}
