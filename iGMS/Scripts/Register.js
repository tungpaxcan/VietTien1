//--------------Click:: Add Roleadmin------------

$('#management').click(function () {
    RoleAdmin();
    $('#management').attr("disabled", true);
})
$('#sales').click(function () {
    Role();
    $('#sales').attr("disabled", true);
})


//------------Register------------
$('#submit').click(function (e) {
   
        Register();
 
})


//-------------Add::RoleAdmin------------
function RoleAdmin() {
    var manageMaincategories = $('#manageMaincategories').is(":checked");
    var purchasemanager = $('#purchasemanager').is(":checked");
    var salesmanager = $('#salesmanager').is(":checked");
    var warehousemanagement = $('#warehousemanagement').is(":checked");
    var managepayments = $('#managepayments').is(":checked");
    var accountingtransfer = $('#accountingtransfer').is(":checked");
    $.ajax({
        url: '/roleadmin/RoleAdmin',
        type: 'post',
        data: {
            manageMaincategories, purchasemanager, salesmanager,
            warehousemanagement, managepayments, accountingtransfer
        },
        success: function (data) {
            if (data.code == 200) {

            }
        }
    })
}


//-------------Add::Role------------
function Role() {
    var admin = $('#admin').is(":checked");
    var editdiscountgoods = $('#editdiscountgoods').is(":checked");
    var editdiscountbill = $('#editdiscountbill').is(":checked");
    var editpricegoods = $('#editpricegoods').is(":checked");
    var editamountgoods = $('#editamountgoods').is(":checked");
    var deletegoods = $('#deletegoods').is(":checked");
    var printagainbill = $('#printagainbill').is(":checked");
    var hangbill = $('#hangbill').is(":checked");
    var changecategoods = $('#changecategoods').is(":checked");
    var changeunit = $('#changeunit').is(":checked");
    var editdate = $('#editdate').is(":checked");
    var identifyconsultants = $('#identifyconsultants').is(":checked");
    var confirmcusinfor = $('#confirmcusinfor').is(":checked");
    var returngoods = $('#returngoods').is(":checked");
    $.ajax({
        url: '/roleadmin/Role',
        type: 'post',
        data: {
            admin, editdiscountgoods, editdiscountbill, editpricegoods,
            editamountgoods, deletegoods, printagainbill, hangbill ,changecategoods,
            changeunit, editdate, identifyconsultants, confirmcusinfor, returngoods
        },
        success: function (data) {
            if (data.code == 200) {

            }
        }
    })
}


//------------Register------------
function Register() {
   
    var id = $('#id').val().trim();
    var user = $('#user').val().trim();
    var pass = $('#pass').val().trim();
    var name = $('#name').val().trim();
    var province = $('#province option:selected').text();
    var district = $('#district option:selected').text();
    var town = $('#town option:selected').text();
    var email = $('#email').val().trim();
    var birth = $('#birth').val().trim();
    var phone = $('#phone').val().trim();
    var des = $('#des').val().trim();
    var address = $('#address').val().trim();
    $('.Loading').css("display","block");
    var format = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/;
    var letter = /[A-Z]/;
    var lower = /[a-z]/;
    var number = /[0-9]/;
    if (name.length <= 0 || user.length<=0) {
        alert("Nhập Tên")
        return;
    }
    if (pass.length < 6) {
        alert("Mật Khẩu Phải Hơn 6 Kí Tự")
        return;
    }
    if (!letter.test(pass)) {
        alert("Mật Khẩu Phải Chứa Chữ Cái Viết Hoa")
        return;
    }
    if (!number.test(pass)) {
        alert("Mật Khẩu Phải Chứa Chữ Số")
        return;
    }
    if (!lower.test(pass)) {
        alert("Mật Khẩu Phải Chứa Chữ Cái Viết Thường")
        return;
    }
    if (!format.test(pass)) {
        alert("Mật Khẩu Phải Chứa Kí Tự Đặc Biệt")
        return;
    }
    if (phone.length < 9 || phone.length > 11) {
        alert("Nhập Đúng Số Điện Thoại")
        return;
    }
    var management = $('#management').is(":checked");
    var sales = $('#sales').is(":checked");
    if (management == false || sales == false) {
        alert("Chưa Xác Nhận Quyền Nhân Viên")
        return;
    }
    $.ajax({
        url: '/register/Register',
        type: 'post',
        data: {
            id, user, pass, name, province, district, town, address, email, birth, phone, des
        },
        success: function (data) {
            if (data.code == 200) {
                Swal.fire({
                    title: "Tạo Tài Khoản Thành Công",
                    icon: "success",
                    buttonsStyling: false,
                    confirmButtonText: "Confirm me!",
                    customClass: {
                        confirmButton: "btn btn-primary"
                    }
                });
                window.location.href = "/Register/Index";
            } else if (data.code == 300) {
                alert(data.msg)
            }
            else {
                alert("Tạo Tài Khoản Thất Bại")
            }
        },
          complete: function () {
              $('.Loading').css("display","none");//Request is complete so hide spinner
        }
    })
}


//---------eye::pass---------
$('.icon-xl.far.fa-eye-slash').css("display", "none")
$('.icon-xl.far.fa-eye').click(function () {
    $('.icon-xl.far.fa-eye-slash').css("display", "block")
    $('.icon-xl.far.fa-eye').css("display", "none")
    $('#pass').attr("type", "text");
})
$('.icon-xl.far.fa-eye-slash').click(function () {
    $('.icon-xl.far.fa-eye-slash').css("display", "none")
    $('.icon-xl.far.fa-eye').css("display", "block")
    $('#pass').attr("type", "password");
})

//-------------barcode-------------
$('#id').keyup(function () {
    var id = $('#id').val().trim();
    JsBarcode("#barcode","NV"+id);
})