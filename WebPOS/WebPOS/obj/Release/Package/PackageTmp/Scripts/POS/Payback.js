
function GetPoints() {
    var monedero = $("#idMonedero").val();
    var Secret = $("#idSecret").val();
    var sventa = $("#sventa").val();

    if (ValidateDatesModedero(monedero, Secret)) {
        var obj = { monederoP: monedero.trim(), secretP: Secret.trim(), totalP: sventa.replace("$", "") };
        var Json = JSON.stringify(obj);
        $.ajax({
            type: "POST",
            url: '/Payback/GetAdminInfo',
            data: "{ 'Jsonvalues': '" + Json + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var idStore = '';
                if (result != null) {

                    var lsLinea = JSON.parse(result);
                    if (lsLinea.ErrorMessage == null) {
                        $("#montoPB").val(lsLinea.Points);
                        $("#GetPaymentModal").modal('hide');
                    } else {
                        MessageError(lsLinea.ErrorMessage);
                    }
                }
                else { MessageError("Error en la peticion"); }
            },
            error: function (xhr) {
                //debugger;
                console.log(xhr.responseText);
                alert("Error has occurred..");
            }
        });
    }

}
function cleanGetPoints() {
    $("#lbmontoPB").val("");
    $("#idMonedero").val("");
    $("#idSecret").val("");
    //var lbmontoPB1 = document.getElementById("montoPB");
    //var lbmontoPBtext = document.getElementById("lbmontoPB");
    //lbmontoPB1.style.display = "none";
    //lbmontoPBtext.style.display = "none";
    document.getElementById("montoPB").value = "";
}
function ValidateDatesModedero(number, secret) {
    if (number.trim() == "" && secret.trim() == "") {
        MessageError("El Monedero/ Nip no puede estar vacio");
        return false;
    } else if (number.trim() == "El Monedero no puede estar vacio") {
        MessageError();
        return false;
    }
    var isVisible = $("#idSecret").is(":visible");
    if (isVisible) {
        if (secret.trim() == "") {
            MessageError("El Nip no puede estar vacio");
            return false;
        }
    } 

    return true;
}
function Onclick_btnCerrarPbk() {
    cleanGetPoints();
    document.getElementById("formapago").value = "Efectivo";
}
//function CierraPopup(idModal) {
//    $("#"+idModal+"").modal('hide');//ocultamos el modal
//    $('body').removeClass('modal-open');//eliminamos la clase del body para poder hacer scroll
//    $('.modal-backdrop').remove();//eliminamos el backdrop del modal
//}