var tableF;

ConsultasF = {
    GetClientesF: function () {
        $('#txtClienteConsultF').attr("autocomplete", true);
        $('#txtClienteConsultF').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/VentasConsultasFranquicias/GetInfoClienteConsultasF",
                    type: "POST",
                    dataType: "json",
                    data: { Prefix: request.term },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { id: item.Id, label: item.Nombre, value: item.Nombre };
                        }))

                    }
                })
            }, select: function (event, ui) {
                $('input[name=txtidClienteConsultaF]').val(ui.item.id);
                ConsultasF.GetPeriodoF(ui.item.id);

                var Folio = null, Periodo = null, Pedido = null;
                if ($('#cmbFolioConsultF').val() > 0) {
                    Folio = $('#cmbFolioConsultF').val();
                }

                if ($('#cmbPeriodoConsultF').val() > 0) {
                    Periodo = $('#cmbPeriodoConsultF').val();
                }

                ConsultasF.GetPedidoBusquedaF($('input[name=txtidClienteConsultaF]').val(), Periodo, '', Folio, 0, null);
            }
        });
    },
    GetPeriodoF: function (idCliente) {
        var clientes = {
            'id': idCliente
        };

        $.ajax({
            type: "POST",
            url: '/VentasConsultasFranquicias/GetPeriodoClienteConsultasF',
            data: "{'clientes':" + JSON.stringify(clientes) + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                $('#cmbPeriodoConsultF').empty();
                $("#cmbPeriodoConsultF").append('<option value=0 >Seleccione un Periodo</option>');
                for (var i = 0; i <= result.length - 1; i++) {
                    $("#cmbPeriodoConsultF").append('<option value=' + result[i].Periodo + '>' + result[i].Periodo + '</option>');
                }
            },
            error: function (xhr) {
                console.log(xhr.responseText);
                alert("Error has ocurred..");
            }
        });
    },
    GetDiaF: function () {
        $('#cmbPeriodoConsultF').change(function () {
            $("#tblConsultaPedidosF").dataTable().empty();

            var periodo = {
                'idCliente': $('input[name=txtidClienteConsultaF]').val(),
                'Periodo': $('#cmbPeriodoConsultF').val()
            };

            $.ajax({
                type: "POST",
                url: '/VentasConsultasFranquicias/GetDiaClienteConsultasF',
                data: "{'periodo':" + JSON.stringify(periodo) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    $('#cmbDiaConsultF').empty();
                    $("#cmbDiaConsultF").append('<option value=0 >Seleccione Dia</option>');
                    for (var i = 0; i <= result.length - 1; i++) {
                        $("#cmbDiaConsultF").append('<option value=' + result[i].dia + '>' + result[i].dia + '</option>');
                    }
                },
                error: function (xhr) {
                    console.log(xhr.responseText);
                    alert("Error has ocurred..");
                }
            });
        });
    },
    GetFolioF: function () {
        $('#cmbDiaConsultF').change(function () {
            var periodo = {
                'idCliente': $('input[name=txtidClienteConsultaF]').val(),
                'Periodo': $('#cmbPeriodoConsultF').val(),
                'Dia': $("#cmbDiaConsultF").val()
            };

            $.ajax({
                type: "POST",
                url: '/VentasConsultasFranquicias/GetFolioClienteConsultasF',
                data: "{'periodo':" + JSON.stringify(periodo) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    $('#cmbFolioConsultF').empty();
                    $("#cmbFolioConsultF").append('<option value=0 >Seleccione Folio</option>');
                    for (var i = 0; i <= result.length - 1; i++) {
                        $("#cmbFolioConsultF").append('<option value=' + result[i].FolioPref + '>' + result[i].FolioPref + '</option>');
                    }
                },
                error: function (xhr) {
                    console.log(xhr.responseText);
                    alert("Error has ocurred..");
                }
            });
        });
    },
    GetPedidoBusquedaF: function (IdCliente, Fecha, Prefijo, Folio, Boton, Pedido) {

        var pedido = {
            'IdCliente': IdCliente,
            'Fecha': Fecha,
            'Prefijo': Prefijo,
            'Folio': Folio,
            'Boton': Boton,
            'Pedido': Pedido
        };

        $.ajax({
            type: "POST",
            url: '/VentasConsultasFranquicias/GetClientePedidoConsultF',
            data: "{'pedido':" + JSON.stringify(pedido) + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                ConsultasF.ShowDataTablePedido(result);
            },
            error: function (xhr) {
                console.log(xhr.responseText);
                alert("Error has ocurred..");
            }
        });
    },
    ShowDataTablePedido: function (result) {
        var dataset = result

        if (dataset != null) {
            var columnDefs = [];
            //loop for populating column defs
            for (let k in dataset[0]) {
                if (columnDefs.indexOf(k) === -1) {
                    columnDefs.push({ width: 200, targets: 0, title: dataset[0][k] });
                }
            }
            //loop for populating data
            var data = [];
            for (let j of dataset) {
                var arr = [];
                for (let k in j) {
                    if (columnDefs.indexOf(k) === -1) {
                        arr.push(j[k]);
                    }
                }
                data.push(arr);
            }

            var spanish = {
                "processing": "Procesando...",
                "lengthMenu": "Mostrar _MENU_ registros",
                "zeroRecords": "No se encontraron resultados",
                "emptyTable": "Ningún dato disponible en esta tabla",
                "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                "search": "Buscar:",
                "infoThousands": ",",
                "loadingRecords": "Cargando...",
                "oPaginate": {
                    "sFirst": "Primero",
                    "sLast": "Último",
                    "sNext": "Siguiente",
                    "sPrevious": "Anterior"
                },
                " oAria ": {
                    " sSortAscending ": ": Activar para ordenar la columna de manera ascendente ",
                    " sSortDescending ": ": Activar para ordenar la columna de manera descendente "
                }
            }

            tableF = $("#tblConsultaPedidosF").dataTable({
                destroy: true,
                language: spanish,
                //searching: false,
                ////scrollX: true,
                //scrollCollapse: true,
                scrollX: true,
                fixedColumns: false,
                select: true,
                data: data,
                columns: [
                    { visible: false, title: "id", },
                    { title: "Venta" },
                    { title: "Abono" },
                    { title: "Fecha Abono" },
                    { title: "Fecha Recibo" },
                    { title: "Monto Pagado" }, //formato
                    { title: "Estatus del Pedido" },
                    { 'data': null, title: 'Pedido', wrap: true, "render": function (item) { return '<div class="btn-group"> <button type="button" id="btnFacturaPdf" onclick="ConsultasF.GetPedidoPDF()" value="0" class="btn btn-success btn-sm" aria-label="Left Align" data-toggle="modal" data-target="#myModal"><span class="fa fa-download"></span> Imprimir</button></div>' } },
                    { 'data': null, title: 'Abono', wrap: true, "render": function (item) { return ' <button type="button" id ="btnAbonoPdf"  onclick="ConsultasF.GetAbonoPDFF()" value="1" class="btn btn-warning btn-sm  aria-label="Left Align" data-toggle="modal" data-target="#myModal""><span class="fa fa-download"></span>Imprimir</button>' } },
                    {
                        "data": null, "render": function (data, type, row) {
                            if (row[9] == true && row[6] != 'Entregado') //Confirmación
                                return "<button type='button' id ='btnConfirmaEntrega'  onclick='ConsultasF.btnConfirmaEntrega()' class='btn btn-info btn-sm'><span class='fa fa-check-circle-o'></span>Confirmación de Entrega</button>";
                            else
                                return ""; //Empty cell content
                        }
                    },

                ],
                "pageLength": 10,
                "lengthChange": false,
                "responsive": true,
                "rowReorder": {
                    "selector": 'td:nth-child(2)'
                },
                "fixedHeader": {
                    "header": true
                }
            });

            $('#tblConsultaPedidosF tbody').on('click', 'tr', function () {
                var data = tableF.api().row(this).data();

            });
        }
        else {
            $(".DTFC_Cloned tbody").empty();
            $("#tblConsultaPedidosF").dataTable().empty();
        }
    },
    
    function() {
        $('#btnPedidoConsultF').click(function () {
            if ($('#txtPedidoConsultF').val() != "") {
                ConsultasF.GetPedidoBusquedaF(null, null, '', null, 1, $('#txtPedidoConsultF').val());
            }
            else {
                alert('Introduzca un folio a buscar');
            }
        });
    },
    GetAbonoPDFF: function () {

        $('#tblConsultaPedidosF tbody').on('click', 'tr', function () {
            var data = tableF.api().row(this).data();
            let URLactual = window.location;
            let Url = URLactual.toString().split('/');

            let rutaReports = 'http://' + Url[2] + '/VentasRPTS/Download_AbonoVentaDormimundo_PDF?IdAbono=' + data[0] + '';
            window.location = rutaReports.replace("''", "");
        });
    },
    GetPedidoPDF: function () {
        $('#tblConsultaPedidosF tbody').on('click', 'tr', function () {
            var data = tableF.api().row(this).data();
            let URLactual = window.location;
            let Url = URLactual.toString().split('/');

            let rutaReports = 'http://' + Url[2] + '/VentasRPTS/Download_PedidoVentaDormimundoFranquicias_PDF?IDVENTA=' + data[0] + '';
            window.location = rutaReports.replace("''", "");
        });
    },
    GetPedidoxFolioPrefijoF: function () {
        $('#cmbFolioConsultF').change(function () {
            ConsultasF.GetPedidoBusquedaF($('input[name=txtidClienteConsultaF]').val(), $('#cmbPeriodoConsultF').val(), $('#cmbFolioConsultF').val().split('-')[0], $('#cmbFolioConsultF').val().split('-')[1], 0, null);
        });
    },
    ClearFiltroF: function () {
        $('#btnNuevoFiltro').click(function () {
            $('#txtClienteConsultF').val('');
            $('#cmbPeriodoConsultF').val(0);
            $('#cmbDiaConsultF').val(0);
            $('#cmbFolioConsultF').val(0);
            $('#txtPedidoConsultF').val('');

            $("#tblConsultaPedidosF").dataTable().empty();
        });
    },
    GetFacturaPDF: function (id, doc, fechaventa) {
        if (id != null) {
            if (doc == 1) {
                doc = "Factura";
            } else { doc = "Complemento"; }


            $.ajax({
                type: 'POST',
                url: '/VentasConsultasFranquicias/GetCFDIF',
                data: "{ 'IdAbono': '" + id + "','Documento':'" + doc + "','fechaventa':'" + fechaventa + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data == "No existe archivo") {
                        MessageErrorF("El pdf que esta intentando accesar aun no se a creado,intentelo mas tarde ...");
                        return;
                    } else {
                        window.location.href = "/VentasConsultas/Download_FactPDF?source=" + data;
                    }
                }
            });
        }
    },
    GetComplementoPDF: function (id) {
        if (id != null) {
            $.ajax({
                type: 'POST',
                url: '/VentasConsultas/GetCFDI',
                data: "{ 'IdAbono': '" + id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data == "No existe archivo") {
                        MessageErrorF("El pdf que esta intentando accesar aun no se a creado,intentelo mas tarde ...");
                        return;
                    } else {
                        window.location.href = "/VentasConsultas/Download_FactPDF?source=" + data;
                    }
                }
            });
        }
    },
    btnConfirmaEntrega: function () {
        $('#tblConsultaPedidosF tbody').on('click', 'tr', function () {
            var data = tableF.api().row(this).data();
            let IdAbono = data[0];
            $.ajax({
                type: "POST",
                url: '/VentasConsultasFranquicias/ConfirmaEntregaF',
                data: "{ 'IdAbono': '" + IdAbono + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var resultado = JSON.parse(result);

                    if (resultado == "true") {
                        AlertNotificationF('Se confirmo con éxito la entrega del pedido', '', 'success');
                        ConsultasF.GetPedidoBusquedaF(null, null, '', null, 1, $('#txtPedidoConsultF').val());
                    }
                    else {
                        AlertNotificationF('Algo salio mal intente nuevamente por favor', '', 'error');
                    }

                },
                error: function (xhr) {
                    console.log(xhr.responseText);
                    alert("Error has ocurred..");
                }
            });
            //var requestOptions = {
            //    method: 'POST',
            //    redirect: 'follow',
            //    headers: new Headers({ 'Content-type': 'application/json' }),
            //    mode: 'no-cors'
            //};

            //fetch("https://localhost:44335/api/ConfirmaEntregaF?IdAbono=" + IdAbono, requestOptions)
            //    .then(response => response.text())
            //    .then(result => console.log('Hola',result))
            //    .catch(error => console.log('error', error));
                

        });
    },
    

}

$(document).ready(function () {
    //ConsultasF.GetClientesF();
    ConsultasF.GetDiaF();
    ConsultasF.GetFolioF();
    //ConsultasF.SearchPedidoF();
    ConsultasF.GetPedidoxFolioPrefijoF();
    ConsultasF.ClearFiltroF();
});


function MessageErrorF(message) {
    toastr.options.timeOut = 6000;
    toastr.warning(message);
    return;
}
function AlertNotificationF(title, text, icon) {
    Swal.fire(title, text, icon)
}

function ClearConsF() {

    $("#cmbPeriodoConsultF")
        .empty()
        .append($("<option></option>")
            .val("0")
            .html("Seleccione Periodo"));
    $("#cmbDiaConsultF")
        .empty()
        .append($("<option></option>")
            .val("0")
            .html("Seleccione Dia"));
    $("#cmbFolioConsultF")
        .empty()
        .append($("<option></option>")
            .val("0")
            .html("Seleccione Folio"));

    $("#txtClienteConsultF").val("");
}