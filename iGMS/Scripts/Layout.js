//--------------------Session::Name--------------
Name();
function Name() {
    $.ajax({
        url: '/home/UserSession',
        type: 'get',
        success: function (data) {
            $('#namesession').empty();
            $('#idsession').empty();
            if (data.code == 200) {
                $('#namesession').append(data.name);
                $('#idsession').append(data.id);
  
            } else (
                alert(data.msg)
            )
        }
    })
}