
var pagenum = $("#pagenum option:selected").val();
var page = 1;
var seach = "";
ListUser(pagenum, page, seach);

//phan trang
$('#page').on('click', 'li', function (e) {
    e.preventDefault();
    page = $(this).attr('id');
    ListUser(pagenum, page,seach);
    
  
});


function ListUser(pagenum,page,seach) {
    $.ajax({
        url: '/register/List',
        type: 'get',
        data: { pagenum,page,seach },
        success: function (data) {
            $('#tbd').empty();
            $('#tbdmodal').empty();
            $('#kt_datatable_info').empty();
            if (data.code == 200) {
                $.each(data.c, function (k, v) {
                    let table = '<tr id="'+v.id+'" role="row" class="odd">';
                    table += ' <td id="barcodeuser'+v.id+'"><svg class="barcode'+v.id+'" id="barcodeuser"></svg></td>';
                    table +='<td>'+v.name+'</td>'
                    table += '<td>'+v.address+'</td>'
                    table += '<td><a class="text-dark-50 text-hover-primary" href="mailto:' + v.email + '">' + v.email + '</a></td>'
                    table += '<td ">' + v.ManageMainCategories + v.PurchaseManager + v.SalesManager + v.WarehouseManagement + v.ManagePayments + v.AccountingTransfer+'</td>'
                    table += '<td class="action" nowrap="nowrap">';
                    table += '<div class="dropdown dropdown-inline">';
                    table += '<a href="javascript:;" class="btn btn-sm btn-clean btn-icon" data-toggle="dropdown">';
                    table += '<i class="la la-cog"></i></a>';
                    table += '<div class="dropdown-menu dropdown-menu-sm dropdown-menu-right">';
                    table += '<ul class="nav nav-hoverable flex-column">';
                    table += ' <li class="nav-item">';
                    table += '<a class="nav-link" href="/Register/EditUser/' + v.id + '">';
                    table += '<i class="nav-icon la la-edit"></i>';
                    table += '<span class="nav-text">Edit Decentralization</span></a></li>';
                    table += '<li class="nav-item"><a class="nav-link" onclick="printDiv(\'barcodeuser'+v.id+'\')">';
                    table += '<i class="nav-icon la la-print"></i><span class="nav-text">Print</span></a></li>';
                    table += '<li class="nav-item"><a class="nav-link" name="delete">';
                    table += '<i class="nav-icon la la-trash"></i><span class="nav-text">Delete</span></a></li>';
                    table += '</ul></div></div>';
                    table += '</td>';
                    table += '</tr>';
                    $('#tbd').append(table);
                    $('#tbdmodal').append(table);
                    JsBarcode(".barcode" + v.id + "", v.id);
                });

                //--------------------------------
                let kt_datatable_info = 'Showing 1 to ' + pagenum + ' of ' + data.count + ' entries'
                $('#kt_datatable_info').append(kt_datatable_info);
                //-----------------------------page---------------------------
                $('#page').empty();
                if (parseInt(page) >= 2) {
                    let pagemin = '<li id="' + 1 + '"><a class="a_1 a_2" >' + 1 + '...</a></li>';
                    $('#page').append(pagemin);
                    let pre = ' <li id="' + (parseInt(page) - 1) +'" class="paginate_button page-item previous disabled" >';
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

$('#pagenum').on('change',function () {
    var pagenum = $("#pagenum option:selected").val();
    var page = 1;
    var seach = "";
    ListUser(pagenum, page,seach)
})


//------------------------tim kiem------------------

$('#seach').on('keyup', function (e) {
        page = 1;
        seach = $('#seach').val();
        ListUser(pagenum, page, seach);
});
$('#print').click(function () {
    $('.modal-xl').css("max-width","100%")
    $('.action').css("display","none")
})
$('#close').click(function () {
    $('.print').css("display", "none");
    $('.action').css("display", "block")
})


//------------------------Delete--------------------
$(document).on('click', "a[name='delete']", function () {
    var id = $(this).closest('tr').attr('id');
    if (confirm("Bạn Muốn Xóa Dữ Liệu Này ???")) {
        $.ajax({
            url: '/register/Delete',
            type: 'post',
            data: {
                id
            },
            success: function (data) {
                if (data.code == 200) {
                    Swal.fire({
                        title: "Xóa Tài Khoản Thành Công",
                        icon: "success",
                        buttonsStyling: false,
                        confirmButtonText: "Confirm me!",
                        customClass: {
                            confirmButton: "btn btn-primary"
                        }
                    });
                    window.location.href = "/Register/ListUser";
                }
                else {
                    alert(data.msg)
                }
            }
        })
}
    })




