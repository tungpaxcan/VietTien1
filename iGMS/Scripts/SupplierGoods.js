var pagenum = $("#pagenum option:selected").val();
var page = 1;
var seach = "";
SupplierGoods(pagenum, page, seach);

//phan trang
$('#page').on('click', 'li', function (e) {
    e.preventDefault();
    page = $(this).attr('id');
    SupplierGoods(pagenum, page, seach);


});


function SupplierGoods(pagenum, page, seach) {
    var idsupplier = $('#idsupplier').val().trim();
    $.ajax({
        url: '/suppliergoods/List',
        type: 'get',
        data: { pagenum, page, seach, idsupplier },
        success: function (data) {
            var Stt = 1;
            $('#tbd').empty();
            $('#kt_datatable_info').empty();
            if (data.code == 200) {
                $.each(data.c, function (k, v) {
                    let table = '<tr id="' + v.id + '" role="row" class="odd">';
                    table += '<td>' + (Stt++) + '</td>'
                    table += '<td id="barcodehh' + v.id + '"><svg class="barcode' + v.id + '" ></svg></td>'
                    table += '<td>' + v.name + '</td>'
                    table += '<td>' + v.unit + '</td>'
                    table += '<td >' + v.purchaseprice + '</td>'
                    table += '<td>' + v.purchasetax + '</td>'
                    table += '<td>' + v.price + '</td>'
                    table += '<td>' + v.pricetax + '</td>'
                    table += '<td class="action" nowrap="nowrap">';
                    table += '<div class="dropdown dropdown-inline">';
                    table += '<a href="javascript:;" class="btn btn-sm btn-clean btn-icon" data-toggle="dropdown">';
                    table += '<i class="la la-cog"></i></a>';
                    table += '<div class="dropdown-menu dropdown-menu-sm dropdown-menu-right">';
                    table += '<ul class="nav nav-hoverable flex-column">';
                    table += ' <li class="nav-item">';
                    table += '<a class="nav-link" href="/Goods/Details/' + v.id + '">';
                    table += '<i class="nav-icon icon-xl la la-building"></i>';
                    table += '<span class="nav-text">Detail</span></a></li>';
                    table += ' <li class="nav-item">';
                    table += '<a class="nav-link" href="/Goods/Edits/' + v.id + '">';
                    table += '<i class="nav-icon la la-edit"></i>';
                    table += '<span class="nav-text">Edit </span></a></li>';
                    table += '<li class="nav-item"><a class="nav-link" onclick="printDiv(\'barcodehh' + v.id + '\')">';
                    table += '<i class="nav-icon la la-print"></i><span class="nav-text">Print</span></a></li>';
                    table += '<li class="nav-item"><a class="nav-link" name="delete">';
                    table += '<i class="nav-icon la la-trash"></i><span class="nav-text">Delete</span></a></li>';
                    table += '</ul></div></div>';
                    table += '</td>';
                    table += '</tr>';

                    $('#tbd').append(table);
                    if (v.id.length == 2) {
                        JsBarcode(".barcode" + v.id, v.id, {
                            format: "EAN2",
                        });
                    } else if (v.id.length == 5) {
                        JsBarcode(".barcode" + v.id, v.id, {
                            format: "EAN5",
                        });
                    } else if (v.id.length == 7 || v.id.length == 8) {
                        JsBarcode(".barcode" + v.id, v.id, {
                            format: "EAN8",
                        });
                    } else if (v.id.length == 12) {
                        JsBarcode(".barcode" + v.id, v.id, {
                            format: "UPC",
                        });
                    } else if (v.id.length == 13) {
                        JsBarcode(".barcode" + v.id, v.id, {
                            format: "EAN13",
                        });
                    } else if (v.id.length == 6) {
                        JsBarcode(".barcode" + v.id, v.id, {
                            format: "pharmacode",
                        });
                    } else {
                        JsBarcode(".barcode" + v.id, v.id, {
                            format: "CODE128",
                        });
                    }
                });

                //--------------------------------
                let kt_datatable_info = 'Showing 1 to ' + pagenum + ' of ' + data.count + ' entries'
                $('#kt_datatable_info').append(kt_datatable_info);
                //-----------------------------page---------------------------
                $('#page').empty();
                if (parseInt(page) >= 2) {
                    let pagemin = '<li id="' + 1 + '"><a class="a_1 a_2" >' + 1 + '...</a></li>';
                    $('#page').append(pagemin);
                    let pre = ' <li id="' + (parseInt(page) - 1) + '" class="paginate_button page-item previous disabled" >';
                    pre += '<a  aria-controls="kt_datatable" data-dt-idx="0" tabindex="0" class="page-link">';
                    pre += '<i class="ki ki-arrow-back"></i></a></li>';
                    $('#page').append(pre);
                }
                for (let i = parseInt(page); i <= (parseInt(page) + 4); i++) {
                    if (i == data.pages + 1) {
                        return;
                    }
                    let li = '<li id="' + i + '" class="paginate_button page-item ">';
                    li += '<a aria-controls="kt_datatable" data-dt-idx="1" tabindex="0" class="page-link">' + i + '</a></li>';
                    $('#page').append(li);


                }


                let next = '<li  id="' + (parseInt(page) + 1) + '" class="paginate_button page-item next" id="kt_datatable_next">';
                next += '<a href="#" aria-controls="kt_datatable" data-dt-idx="6" tabindex="0" class="page-link"><i class="ki ki-arrow-next"></i></a></li>';
                $('#page').append(next);

                let pagemax = '<li id="' + data.pages + '"><a class="a_1 a_2" >...' + data.pages + '</a></li>';
                $('#page').append(pagemax);

            } else (
                alert(data.msg)
            )
        }
    })
}

$('#pagenum').on('change', function () {
    var pagenum = $("#pagenum option:selected").val();
    var page = 1;
    var seach = "";
    SupplierGoods(pagenum, page, seach)
})


//------------------------tim kiem------------------

$('#seach').on('keyup', function (e) {
    page = 1;
    seach = $('#seach').val();
    SupplierGoods(pagenum, page, seach);
});





