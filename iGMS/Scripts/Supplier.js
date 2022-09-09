var pagenum = $("#pagenum option:selected").val();
var page = 1;
var seach = "";
Supplier(pagenum, page, seach);

//phan trang
$('#page').on('click', 'li', function (e) {
    e.preventDefault();
    page = $(this).attr('id');
    Supplier(pagenum, page, seach);


});


function Supplier(pagenum, page, seach) {
    $.ajax({
        url: '/supplier/List',
        type: 'get',
        data: { pagenum, page, seach },
        success: function (data) {
            $('#tbd').empty();
            $('#kt_datatable_info').empty();
            if (data.code == 200) {
                $.each(data.c, function (k, v) {
                    let table = '<tr id="' + v.id + '" role="row" class="odd">';
                    table += '<td>' + v.id + '</td>'
                    table += '<td>' + v.name + '</td>'
                    table += '<td>' + v.address + '</td>'
                    table += '<td class="action" nowrap="nowrap">';
                    table += '<div class="dropdown dropdown-inline">';
                    table += '<a href="javascript:;" class="btn btn-sm btn-clean btn-icon" data-toggle="dropdown">';
                    table += '<i class="la la-cog"></i></a>';
                    table += '<div class="dropdown-menu dropdown-menu-sm dropdown-menu-right">';
                    table += '<ul class="nav nav-hoverable flex-column">';
                    table += ' <li class="nav-item">';
                    table += '<a class="nav-link" href="/SupplierGoods/Index/' + v.id + '">';
                    table += '<i class="nav-icon icon-xl la la-building"></i>';
                    table += '<span class="nav-text">Sản Phẩm Của Nhà Cung Cấp</span></a></li>';
                    table += ' <li class="nav-item">';
                    table += '<a class="nav-link" href="/Supplier/Details/' + v.id + '">';
                    table += '<i class="nav-icon icon-xl la la-building"></i>';
                    table += '<span class="nav-text">Detail</span></a></li>';
                    table += ' <li class="nav-item">';
                    table += '<a class="nav-link" href="/Supplier/Edits/' + v.id + '">';
                    table += '<i class="nav-icon la la-edit"></i>';
                    table += '<span class="nav-text">Edit </span></a></li>';
                    table += '<li class="nav-item"><a class="nav-link" onclick="printDiv(\'' + v.id + '\')">';
                    table += '<i class="nav-icon la la-print"></i><span class="nav-text">Print</span></a></li>';
                    table += '<li class="nav-item"><a class="nav-link" name="delete">';
                    table += '<i class="nav-icon la la-trash"></i><span class="nav-text">Delete</span></a></li>';
                    table += '</ul></div></div>';
                    table += '</td>';
                    table += '</tr>';
                    $('#tbd').append(table);
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
    Supplier(pagenum, page, seach)
})


//------------------------tim kiem------------------

$('#seach').on('keyup', function (e) {
    page = 1;
    seach = $('#seach').val();
    Supplier(pagenum, page, seach);
});

//----------------Add::Supplier---------------------
function Add() {
    var id = $('#id').val().trim();
    var name = $('#name').val().trim();
    var nametransaction = $('#nametransaction').val().trim();
    var address = $('#address').val().trim();
    var represent = $('#represent').val().trim();
    var position = $('#position').val().trim();
    var taxcode = $('#taxcode').val().trim();
    var phone = $('#phone').val().trim();
    var fax = $('#fax').val().trim();
    var email = $('#email').val().trim();
    var website = $('#website').val().trim();
    var stk = $('#stk').val().trim();
    var bank = $('#bank').val().trim();
    var groupgoods = $("#groupgoods option:selected").val();
    var des = $('#des').val().trim();
    $('.Loading').css("display", "block");
    if (name.length <= 0) {
        alert("Nhập Tên Nhà Cung Cấp")
        return;
    } if (represent.length <= 0) {
        alert("Nhập Tên Người Đại Diện")
        return;
    } if (taxcode.length <= 0) {
        alert("Nhập Mã Số Thuê")
        return;
    } if (fax.length <= 0) {
        alert("Nhập Số Fax")
        return;
    } if (phone.length < 9 || phone.length > 11) {
        alert("Nhập Đúng Số Điện Thoại")
        return;
    } if (validateEmail(email)) {
        alert("Nhập Đúng Email")
        return;
    } if (stk.length<=0) {
        alert("Nhập Số Tài Khoản")
        return;
    } if (bank.length <= 0) {
        alert("Nhập Ngân Hàng")
        return;
    } if (groupgoods == -1) {
        alert("Chọn Nhóm Hàng")
        return;
    } if (id.length <= 0) {
        alert("Nhập Mã NCC")
        return;
    }
    $.ajax({
        url: '/supplier/Add',
        type: 'post',
        data: {
            id, name, nametransaction, address, represent, position,
            taxcode, phone, fax, email, website, stk, bank, groupgoods, des
        },
        success: function (data) {
            if (data.code == 200) {
                Swal.fire({
                    title: "Tạo Nhà Cung Cấp Thành Công",
                    icon: "success",
                    buttonsStyling: false,
                    confirmButtonText: "Confirm me!",
                    customClass: {
                        confirmButton: "btn btn-primary"
                    }
                });
                window.location.href = "/Supplier/Index";
            } else if (data.code == 300) {
                alert(data.msg)
            }
            else {
                alert("Tạo Nhà Cung Cấp Thất Bại")
            }
        },
        complete: function () {
            $('.Loading').css("display", "none");//Request is complete so hide spinner
        }
    })
}

//----------------Edit::CateGoods---------------------
function Edit() {
    var id = $('#id').val().trim();
    var name = $('#name').val().trim();
    var nametransaction = $('#nametransaction').val().trim();
    var address = $('#address').val().trim();
    var represent = $('#represent').val().trim();
    var position = $('#position').val().trim();
    var taxcode = $('#taxcode').val().trim();
    var phone = $('#phone').val().trim();
    var fax = $('#fax').val().trim();
    var email = $('#email').val().trim();
    var website = $('#website').val().trim();
    var stk = $('#stk').val().trim();
    var bank = $('#bank').val().trim();
    var groupgoods = $("#groupgoods option:selected").val();
    var des = $('#des').val().trim();
    $('.Loading').css("display", "block");
    if (name.length <= 0) {
        alert("Nhập Tên Nhà Cung Cấp")
        return;
    } if (represent.length <= 0) {
        alert("Nhập Tên Người Đại Diện")
        return;
    } if (taxcode.length <= 0) {
        alert("Nhập Mã Số Thuê")
        return;
    } if (phone.length < 9 || phone.length > 11) {
        alert("Nhập Đúng Số Điện Thoại")
        return;
    } if (validateEmail(email)) {
        alert("Nhập Đúng Email")
        return;
    } if (stk.length <= 0) {
        alert("Nhập Số Tài Khoản")
        return;
    } if (bank.length <= 0) {
        alert("Nhập Ngân Hàng")
        return;
    } if (groupgoods == -1) {
        alert("Chọn Nhóm Hàng")
        return;
    }
    if (fax.length <= 0) {
        alert("Nhập Số Fax")
        return;
    }
    $.ajax({
        url: '/supplier/Edit',
        type: 'post',
        data: {
            id, name, nametransaction, address, represent, position,
            taxcode, phone, fax, email, website, stk, bank, groupgoods, des
        },
        success: function (data) {
            if (data.code == 200) {
                Swal.fire({
                    title: "Sửa Nhà Cung Cấp Thành Công",
                    icon: "success",
                    buttonsStyling: false,
                    confirmButtonText: "Confirm me!",
                    customClass: {
                        confirmButton: "btn btn-primary"
                    }
                });
                window.location.href = "/Supplier/Index";
            } else if (data.code == 300) {
                alert(data.msg)
            }
            else {
                alert("Sửa Nhà Cung Cấp Thất Bại")
            }
        },
        complete: function () {
            $('.Loading').css("display", "none");//Request is complete so hide spinner
        }
    })
}

//----------------Delete::CateGoods---------------------
$(document).on('click', "a[name='delete']", function () {
    var id = $(this).closest('tr').attr('id');
    if (confirm("Bạn Muốn Xóa Dữ Liệu Này ???")) {
        $.ajax({
            url: '/supplier/Delete',
            type: 'post',
            data: {
                id
            },
            success: function (data) {
                if (data.code == 200) {
                    Swal.fire({
                        title: "Xóa Nhà Cung Cấp Thành Công",
                        icon: "success",
                        buttonsStyling: false,
                        confirmButtonText: "Confirm me!",
                        customClass: {
                            confirmButton: "btn btn-primary"
                        }
                    });
                    window.location.href = "/Supplier/Index";
                }
                else {
                    alert(data.msg)
                }
            }
        })
    }
})

//-------------barcode-------------
$('#id').keyup(function () {
    var id = $('#id').val().trim();
    JsBarcode("#barcode", "NCC" + id);
})