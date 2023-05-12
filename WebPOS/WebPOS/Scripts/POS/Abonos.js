function SearchPedido() {
    var folio = $("#folipedido").val();
    $.ajax({
        type: "POST",
        url: '/Abonos/Buscapedido',
        data: "{ 'folio': '" + folio + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#row").closest('tr').remove();
            var resultado = JSON.parse(result);
            console.log(resultado);
            if (result == "[]" || result == "null" || result == null ) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Para generar un abono necesita facturar el pedido'
                })
                return;
            }
            if (resultado[0].Id == "0") {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Pedido cancelado'
                })
                return;
            }
            if (result != "null") {

                var table = $("#tblAbono");
                var LineaPed = JSON.parse(result);
                var idventa = LineaPed[0].IDVenta;
            
                $("#sventa").val(LineaPed[0].PorPagar);
                //$("#GetPaymentModal").modal({ backdrop: 'static', keyboard: false });
                $("#idMonedero").val(LineaPed[0].Monedero);
                $("#idSecret").val(LineaPed[0].SecretPassword);
                var i = 1
                var i = $('#tblAbono tr').length;
                //console.log(new Intl.NumberFormat('es-MX').format(LineaPed[0].PorPagar));
                table.append('<tr id="row"> <td>' + LineaPed[0].Id+ '</td>' +
                    '<td>' + LineaPed[0].IDVenta + '</td>' +
                    '<td>' + LineaPed[0].Fecha + '</td>' +
                    '<td>' + LineaPed[0].Vendedor + '</td>' +
                    '<td>'+LineaPed[0].Pagado + '</td> ' +
                    '<td> ' + LineaPed[0].PorPagar + '</td >'+ 
                    '<td> ' + LineaPed[0].Total + '</td>' +
                    '<td><select class="form-control col-md-12 form-control-sm mb-3" id="formadepago" onchange="FormaPagoAbonos()"><option value="">Seleccione Forma de Pago</option><option value="Efectivo">Efectivo</option><option value="Terminal Banamex">Terminal Banamex</option><option value="Terminal Bancomer">Terminal Bancomer</option><option value="American Express">American Express</option><option value="Terminal PROSA">Terminal PROSA</option><option value="Terminal Santander">Terminal Santander</option><option value="Deposito / Cheque">Deposito / Cheque</option><option value="Transferencia Electronica">Transferencia Electronica</option><option value="Transf. Elect. Dormicredit St.">Transf. Elect. Dormicredit St.</option><option value="Monedero Payback">Monedero Payback</option><option value="Compensación">Compensación</option> <option value="Anticipo">Anticipo</option></select></td>'+
                    '<td> <select class= "form-control col-md-10 form-control-sm mb-3" id = "seltarjeta" ><option value="">Seleccione Tipo de Tarjeta</option><option value="Credito">Credito</option><option value="Debito">Debito</option></select > ' + '</td > <td>' + '   <input class="form-control col-md-10 form-control-sm mb-3" type="text" placeholder=""  maxlength="4" id="terminatarjeta" onKeypress="if (event.keyCode < 45 || event.keyCode > 57) event.returnValue = false;">' + '</td> <td> ' + ' <input class="form-control col-md-12 form-control-sm mb-3" type="text" placeholder="" id="montop" onKeypress="if (event.keyCode < 45 || event.keyCode > 57) event.returnValue = false;">' + '</td><td>' + '  <input class="form-control col-md-10 form-control-sm mb-3" type="date" id="fechapago">' + '</td><td hidden="hidden">' + LineaPed[0].CorreoCliente + '</td> <td hidden="hidden">' + LineaPed[0].CorreoUsuario + '</td>' +                    
                    '<td> <textarea id="observacionesNoFactura"></textarea></td>' +
                    '<td hidden="hidden">' + LineaPed[0].MetodoPago33 + '</td>'+
                    '<td hidden = "hidden" > ' + LineaPed[0].Parcialidad + '</td>' +
                    '<td><div class="form-check" style="padding-top: 13px; padding-left: 6px;"><input class="form-check form-check-inline" type="checkbox" value="ok" id="abonook" onchange="ChecListo()"></div> </td></tr > ');

            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Para generar un abono necesita facturar el pedido'
                })
            }
        },
        error: function (xhr) {
            //debugger;
            console.log(xhr.responseText);
            alert("Error has occurred..");
        }
    });
}

$(document).ready(function () {
    //if (navigator.geolocation) { //check if geolocation is available
    //    navigator.geolocation.getCurrentPosition(function (position) {
    //        console.log(position);
    //    });
    //}

});

function AddAbono() {
    event.preventDefault();
    var errormessage = "";
    var idCheck = document.getElementById('abonook');
    if (idCheck == null) { return; }
    var formapago = document.getElementById("formadepago").value;
    var montop = document.getElementById("montop").value;
    var FechaPago = $('#fechapago').val();
    var observacionesNoFactura = $('#observacionesNoFactura').val();
    let importado = 0;    

    if (formapago == "Anticipo") {
        importado = 1;
    }

    if (idCheck.checked == false) {
        errormessage = 'No ha seleccionado ninguna venta para abonar';
        errorspagos(errormessage);
        return;
    }
    if (formapago == "") {
        errormessage = 'Primero seleccione la forma de pago';
        errorspagos(errormessage);
        return;
    }
    if (montop == "") {
        errormessage = 'El monto debe ser mayor a 0';
        errorspagos(errormessage);
        return;
    }
    if (FechaPago == "") {
        errormessage = "Debe seleccionar la fecha de pago";
        errorspagos(errormessage);
        return;
    }
    if (Montopayv()) { } else { return;}
    //CheckAcumulation();
    var validatarjeta = FormaPagoAb(formapago);
    if (validatarjeta == false) { return;}
    var tipotarjeta = document.getElementById("seltarjeta").value;
    var terminatarjeta = document.getElementById("terminatarjeta").value;

    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: true
    })
    swalWithBootstrapButtons.fire({
        title: 'Confirma Abono',
        text: "¿Estás seguro?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Si Abonar',
        cancelButtonText: 'No, Cancelar',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            var TableAbono = new Array();
            $('#tblAbono tr').each(function (row, tr) {
                TableAbono[row] = {
                    "Id": $(tr).find('td:eq(0)').text(),
                    "IDVenta": $(tr).find('td:eq(1)').text(),
                    "Fecha": $(tr).find('td:eq(2)').text(),
                    "Vendedor": $(tr).find('td:eq(3)').text(),
                    "Pagado": $(tr).find('td:eq(4)').text().replace('$', ''),
                    "PorPagar": $(tr).find('td:eq(5)').text().replace('$', ''),
                    "Total": $(tr).find('td:eq(6)').text().replace('$', ''),
                    "FormaDePago": formapago,
                    "TipoTarjeta": tipotarjeta,
                    "TerminacionTarjeta": terminatarjeta,
                    "Monto": montop,
                    "FormaPago33": formapago,
                    "TipoComp33": "P",
                    "UsoCFDI33": "P01",
                    "TipoRel33": "8",
                    "CFDI_Rel33": $(tr).find('td:eq(1)').text(),
                    "CorreoCliente": $(tr).find('td:eq(12)').text(),
                    "CorreoUsuario": $(tr).find('td:eq(13)').text(),
                    "MetodoPago33": $(tr).find('td:eq(14)').text(),
                    "Parcialidad": Number($(this).find("td").eq(16).html()) + 1,
                    "FechaPago": FechaPago,
                    "Importado": importado,
                    "comentariosNoTimbrar": observacionesNoFactura
                }
            });
            TableAbono.shift();// End
            var idstore = $('#idstore').val();
            var WhsID = $('#WhsID').val();
            var monedero = $("#idMonedero").val();
            var Secret = $("#idSecret").val();
            var Jsonpago = {
                "Id": TableAbono[0].Id,
                "IDVenta": TableAbono[0].IDVenta,
                "Fecha": TableAbono[0].Fecha,
                "Vendedor": TableAbono[0].Vendedor,
                "Pagado": TableAbono[0].Pagado,
                "PorPagar": TableAbono[0].PorPagar,
                "Total": TableAbono[0].Total,
                "FormaDePago": TableAbono[0].FormaDePago,
                "TipoTarjeta": TableAbono[0].TipoTarjeta,
                "TerminacionTarjeta": TableAbono[0].TerminacionTarjeta,
                "Monto": TableAbono[0].Monto,
                "Idstore": idstore,
                "Afiliacion": "",
                "WhsID": WhsID,
                "Monedero": monedero,
                "SecretPassword": Secret,
                "FormaPago33": formapago,
                "MetodoPago33": TableAbono[0].MetodoPago33,
                "Parcialidad": TableAbono[0].Parcialidad,
                "TipoComp33": TableAbono[0].TipoComp33,
                "UsoCFDI33": TableAbono[0].UsoCFDI33,
                "TipoRel33": TableAbono[0].TipoRel33,
                "CFDI_Rel33": TableAbono[0].CFDI_Rel33,
                "CorreoCliente": TableAbono[0].CorreoCliente,
                "CorreoUsuario": TableAbono[0].CorreoUsuario,
                "FechaPago": TableAbono[0].FechaPago,                
                "Importado": TableAbono[0].Importado,
                "comentariosNoTimbrar": TableAbono[0].comentariosNoTimbrar,

            }
            Data = JSON.stringify(Jsonpago);
            $.ajax({
                type: "POST",
                url: '/Abonos/AddAbono',
                data: "{ 'Abono': '" + Data + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var result = JSON.parse(msg);
                    $("#row").closest('tr').remove();
                    $("#folipedido").val('');

                    var lbmontoPB1 = document.getElementById("montoPB");
                    var lbmontoPBtext = document.getElementById("montopaybacklb");
                    lbmontoPB1.style.display = "none";
                    lbmontoPBtext.style.display = "none";
                    if (result.payresponse == false) {
                        if (result.Monedero != null || result.Monedero != "") {
                            errorspagos(result.Monedero);
                            return;
                        }
                        else { 

                        errorspagos("Monto erroneo de la venta");
                            return;
                        }
                    }
                    //para productivo

                    let URLactual = window.location;
                    let Url = URLactual.toString().split('/');
                    let rutaReports = 'http://' + Url[2] + '/AbonosRPTS/Download_Abono_PDF?IDABONO=' + result.Id;
                    window.location = rutaReports.replace("''", "");
                 
                    //para local 
                    //window.location.href = "/AbonosRPTS/Download_Abono_PDF?IDABONO=" + result.Id;

                    if (result.Monedero == null || result.Monedero == "" || result.Monedero == "null") {
                        swalWithBootstrapButtons.fire(
                            'Abono exitoso!',
                            '¿Nuevo abono?',
                            'success'
                        )
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Abono exitoso!',
                            '¿Nuevo abono?, Oops hubo un conflicto al acomular puntos:' + result.Monedero,
                            'success'
                        )
                    }
                   
                }//success
            });

        } else if (
            /* Read more about handling dismissals below */
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Proceso Cancelado',
                'Revisa tus datos',
                'error'
            )
        }
    })
}

function errorspagos(errormessage) {
    var lbmontoPB1 = document.getElementById("montoPB");
    var lbmontoPBtext = document.getElementById("montopaybacklb");
    lbmontoPB1.style.display = "none";
    lbmontoPBtext.style.display = "none";
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: errormessage
    })
}

function FormaPagoAb(formapago) {
    var tipotarjeta = document.getElementById("seltarjeta").value;
    var terminatarjeta = document.getElementById("terminatarjeta").value;
    switch (formapago) {
        case "Terminal PROSA":
        case "American Express":
        case "Terminal Bancomer":
        case "Terminal Santander":
        case "Terminal Banamex":
            if (tipotarjeta == "") {
                errormessage = 'Seleccione el tipo de tarjeta';
                errorspagos(errormessage);
                return false;
            }
            if (terminatarjeta == "") {
                errormessage = 'Seleccione terminacion de tarjeta/cuenta de la venta';
                errorspagos(errormessage);
                return false;
            }

            break;
        case "Efectivo":
        case "Deposito / Cheque":
        case "Transferencia Electronica":
        case "Transf. Elect. Dormicredit St.":
            break;
        default:

    }
}

$(document).ready(function () {
     var path = '/Abonos/SearchCli';

});

//$("#ddlClientes").change(function () {
    //var Idcliente = $("#ddlClientes option:selected").val();
$("#datalistcliente").on('input', function () {
    var val = $('#datalistcliente').val();
    var Idcliente = $('#datalistOptincli').find('option[value="' + val + '"]').data('code');
    $.ajax({
        type: "POST",
        url: '/Abonos/GetPeriods',
        data: "{ 'idcliente': '" + Idcliente + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result != null) {
                
                var lsper = JSON.parse(result);
                $("#ddlPeriodoF")
                    .empty()
                    .append($("<option></option>")
                        .val("0")
                        .html(":.Seleccione.:"));
                $.each(lsper, function (key, value) {
                    var option = $(document.createElement('option'));
                    option.html(value.Periodo);
                    option.val(value.Periodo);
                    $("#ddlPeriodoF")
                        .append(option);
                });

            }
            return false;
        },
        error: function (xhr) {
            //debugger;
            console.log(xhr.responseText);
            alert("Error has occurred..");
        }
    });
});

$("#ddlPeriodoF").change(function () {
    $('#datalistcliente').prop('disabled', 'disabled');
    var fecha = $("#ddlPeriodoF option:selected").val();
    var val = $('#datalistcliente').val();
    var Idcliente = $('#datalistOptincli').find('option[value="' + val + '"]').data('code');
    //var Idcliente = $("#ddlClientes option:selected").val();
    $.ajax({
        type: "POST",
        url: '/Abonos/GetDays',
        data: "{ 'idcliente': '" + Idcliente + "', 'date': '" + fecha + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result != null) {

                var lsper = JSON.parse(result);
                $("#ddlDiaF")
                    .empty()
                    .append($("<option></option>")
                        .val("0")
                        .html(":.Seleccione.:"));
                $.each(lsper, function (key, value) {
                    var option = $(document.createElement('option'));
                    option.html(value.Dia);
                    option.val(value.Dia);
                    $("#ddlDiaF")
                        .append(option);
                });
            }
            return false;
        },
        error: function (xhr) {
            //debugger;
            console.log(xhr.responseText);
            alert("Error has occurred..");
        }
    });
});

$("#ddlDiaF").change(function () {
    var fecha = $("#ddlPeriodoF option:selected").val();
    var val = $('#datalistcliente').val();
    var Idcliente = $('#datalistOptincli').find('option[value="' + val + '"]').data('code');
    //var Idcliente = $("#ddlClientes option:selected").val();
    var dia = $("#ddlDiaF option:selected").val();
    $.ajax({
        type: "POST",
        url: '/Abonos/GetFolios',
        data: "{ 'idcliente': '" + Idcliente + "', 'date': '" + fecha + "' ,'day': '" + dia +" '}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result != null) {

                var lsper = JSON.parse(result);
                $("#ddlFolioF")
                    .empty()
                    .append($("<option></option>")
                        .val("0")
                        .html(":.Seleccione.:"));
                $.each(lsper, function (key, value) {
                    var option = $(document.createElement('option'));
                    option.html(value.FolioPref);
                    option.val(value.Folio);
                    $("#ddlFolioF")
                        .append(option);
                });
            }
            return false;
        },
        error: function (xhr) {
            //debugger;
            console.log(xhr.responseText);
            alert("Error has occurred..");
        }
    });
});

$("#ddlFolioF").change(function () {
    var fecha = $("#ddlPeriodoF option:selected").val();
    var val = $('#datalistcliente').val();
    var Idcliente = $('#datalistOptincli').find('option[value="' + val + '"]').data('code');
    //var Idcliente = $("#ddlClientes option:selected").val();
    var dia = $("#ddlDiaF option:selected").val();
    var folio = $("#ddlFolioF option:selected").val();
    $.ajax({
        type: "POST",
        url: '/Abonos/Buscapedido',
        data: "{ 'idcliente': '" + Idcliente + "', 'date': '" + fecha + "' ,'day': '" + dia + " ', 'folio': '" + folio +" '}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#row").closest('tr').remove();
            if (result == "[]") {

                return;
            }
            if (result != "null") {
  

                var table = $("#tblAbono");
                var LineaPed = JSON.parse(result);
                var idventa = LineaPed[0].IDVenta;
                $("#sventa").val(LineaPed[0].PorPagar);
                $("#idMonedero").val(LineaPed[0].Monedero);
                $("#idSecret").val(LineaPed[0].SecretPassword);
                var i = 1
                var i = $('#tblAbono tr').length;

                table.append('<tr id="row"> <td>' + LineaPed[0].Id + '</td>' +
                    '<td>' + LineaPed[0].IDVenta + '</td>' +
                    '<td>' + LineaPed[0].Fecha + '</td>' +
                    '<td>' + LineaPed[0].Vendedor + '</td>' +
                    '<td>' + LineaPed[0].Pagado + '</td> ' +
                    '<td> ' + LineaPed[0].PorPagar + '</td >' +
                    '<td> ' + LineaPed[0].Total + '</td>' +
                    '<td><select class="form-control col-md-12 form-control-sm mb-3" id="formadepago" onchange="FormaPagoAbonos()"><option value="">Seleccione Forma de Pago</option><option value="Efectivo">Efectivo</option><option value="Terminal Banamex">Terminal Banamex</option><option value="Terminal Bancomer">Terminal Bancomer</option><option value="American Express">American Express</option><option value="Terminal PROSA">Terminal PROSA</option><option value="Terminal Santander">Terminal Santander</option><option value="Deposito / Cheque">Deposito / Cheque</option><option value="Transferencia Electronica">Transferencia Electronica</option><option value="Transf. Elect. Dormicredit St.">Transf. Elect. Dormicredit St.</option><option value="Monedero Payback">Monedero Payback</option><option value="Compensación">Compensación</option> <option value="Anticipo">Anticipo</option></select></td>' +
                    '<td> <select class= "form-control col-md-10 form-control-sm mb-3" id = "seltarjeta" ><option value="">Seleccione Tipo de Tarjeta</option><option value="Credito">Credito</option><option value="Debito">Debito</option></select > ' + '</td > <td>' + '   <input class="form-control col-md-10 form-control-sm mb-3" type="text" placeholder=""  maxlength="4" id="terminatarjeta" onKeypress="if (event.keyCode < 45 || event.keyCode > 57) event.returnValue = false;">' + '</td> <td> ' + ' <input class="form-control col-md-12 form-control-sm mb-3" type="text" placeholder="" id="montop" onKeypress="if (event.keyCode < 45 || event.keyCode > 57) event.returnValue = false;">' + '</td><td>' + '  <input class="form-control col-md-10 form-control-sm mb-3" type="date" id="fechapago">' + '</td><td hidden="hidden">' + LineaPed[0].CorreoCliente + '</td> <td hidden="hidden">' + LineaPed[0].CorreoUsuario + '</td>' +                    
                    '<td> <textarea id="observacionesNoFactura2"></textarea></td>' +
                    '<td hidden="hidden">' + LineaPed[0].MetodoPago33 + '</td>' +
                    '<td hidden = "hidden" > ' + LineaPed[0].Parcialidad + '</td>' +
                    '<td><div class="form-check" style="padding-top: 13px; padding-left: 6px;"><input class="form-check form-check-inline" type="checkbox" value="ok" id="abonook" onchange="ChecListo()"></div> </td></tr > ');
            }
            else {
                //alert("No existe datos");
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Para generar un abono necesita facturar el pedido'
                })
            }
        },
        error: function (xhr) {
            //debugger;
            console.log(xhr.responseText);
            alert("Error has occurred..");
        }
    });
});

function Clearabonos() {
    $("#datalistcliente").val('');
    $("#ddlPeriodoF")
        .empty()
        .append($("<option></option>")
            .val("0")
            .html("Seleccione Periodo"));
    $("#ddlDiaF")
        .empty()
        .append($("<option></option>")
            .val("0")
            .html("Seleccione Dia"));
    $("#ddlFolioF")
        .empty()
        .append($("<option></option>")
            .val("0")
            .html("Seleccione Folio"));
    $('#datalistcliente').prop('disabled', false);
}
function FormaPagoAbonos() {
    var opt = document.querySelector('#formadepago option:checked');
    var formapago = opt.value;
    var lbmontoPB1 = document.getElementById("montoPB");
    var lbmontoPBtext = document.getElementById("montopaybacklb");
    switch (formapago) {
        case "Terminal PROSA":
        case "American Express":
        case "Terminal Bancomer":
        case "Terminal Santander":
        case "Terminal Banamex":
        case "Efectivo":
        case "Deposito / Cheque":
        case "Transferencia Electronica":
        case "Transf. Elect. Dormicredit St.":
            lbmontoPB1.style.display = "none";
            lbmontoPBtext.style.display = "none";
            break;
        case "Monedero Payback":
            lbmontoPB1.style.display = "block";
            lbmontoPBtext.style.display = "block";
            cleanGetPoints();
            $("#GetPaymentModal").modal({ backdrop: 'static', keyboard: false });
            break;
        default:

    }
}

function ChecListo() {
    CheckAcumulation();
}

function Montopayv() {
    var TableAbono = new Array();
    $('#tblAbono tr').each(function (row, tr) {
        TableAbono[row] = {
            "Id": $(tr).find('td:eq(0)').text(),
            "IDVenta": $(tr).find('td:eq(1)').text(),
            "Fecha": $(tr).find('td:eq(2)').text(),
            "Vendedor": $(tr).find('td:eq(3)').text(),
            "Pagado": $(tr).find('td:eq(4)').text(),
            "PorPagar": $(tr).find('td:eq(5)').text(),
            "Total": $(tr).find('td:eq(6)').text()
        }
    });
    TableAbono.shift();// End
    var PorPagar = TableAbono[0].PorPagar;
    var montoPayback = $("#montoPB").val();
    var opcionpago = document.querySelector('#formadepago option:checked');
    if (opcionpago.value == "Monedero Payback") {
        if (montoPayback == "") {
            errormessage = 'El monto a pagar no puede ser mayor al monto Payback';
            errorspagos(errormessage);
            return false;
        } else if (parseInt(PorPagar) > parseInt(montoPayback)) {
            errormessage = 'El monto a pagar no puede ser mayor al monto Payback';
            errorspagos(errormessage);
            return false;
        }
        return true;
    }
    else {
        return true;
    }
}