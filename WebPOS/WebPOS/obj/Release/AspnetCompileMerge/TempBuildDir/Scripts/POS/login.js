login = {
    modalLogin: function () {
        $("#ext-gen11").click(function () {
            $("#LoginModal").modal({ backdrop: 'static', keyboard: false });
            login.Acceso();
        });
    },
    Acceso: function () {
        $("#btnAceptar").click(function (e) {
            e.preventDefault();

            var user = $("#txtUsername2").val().toString();
            var password = $("#txtPassword2").val().toString();

            if (user.trim() === "" || password.trim() === "") {
                alert("Introduzca el usuario/password");
                return true;
            }
            else
            {
                $("#LoginModal").modal("hide");

                var Users = {
                    "usuario": user,
                    "password": password
                };

                var stringifydata = JSON.stringify(login);
                $.ajax({
                    type: "POST",
                    url: '/Login/login',
                    data: JSON.stringify(Users),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {

                        var data = JSON.parse(result);

                        if (data != null) {
                            if (data.Status == 'I') {
                                alert("Su cuenta ha sido inactivada. Pongase en contacto con el administrador del sistema para que le solicite la reactivación de la misma");
                            }
                            else {
                                window.location.href = '/Home/Index';
                            }
                        }
                        else {
                            alert('Para poder ingresar al sistema debera estar inscrito. Pregunta por una suscripcion al Administrador del Sistema');
                        }

                        return false;
                    },
                    error: function (xhr) {
                        //debugger;
                        console.log(xhr.responseText);
                        alert("Para poder ingresar al sistema debera estar inscrito. Pregunta por una suscripcion al Administrador del Sistema");
                    }
                })
            }

        });
    },

};

usuario = {
    crear: function (nombre,apellid) {

    }
}

$(document).ready(function () {
    login.modalLogin();
    login.Acceso();
}
);
