var table;

Consultas = {
    GetClientes: function () {
        $('#txtClienteConsult').attr("autocomplete", true);
        $('#txtClienteConsult').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/VentasConsultas/GetInfoClienteConsultas",
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
                $('input[name=txtidClienteConsulta]').val(ui.item.id);
                Consultas.GetPeriodo(ui.item.id);

                var Folio = null, Periodo = null, Pedido = null;
                if ($('#cmbFolioConsult').val() > 0) {
                    Folio = $('#cmbFolioConsult').val();
                }

                if ($('#cmbPeriodoConsult').val() > 0) {
                    Periodo = $('#cmbPeriodoConsult').val();
                }

                Consultas.GetPedidoBusqueda($('input[name=txtidClienteConsulta]').val(), Periodo, '', Folio, 0, null);
            }
        });
    },
    GetPeriodo: function (idCliente) {
        var clientes = {
            'id': idCliente
        };

        $.ajax({
            type: "POST",
            url: '/VentasConsultas/GetPeriodoClienteConsultas',
            data: "{'clientes':" + JSON.stringify(clientes) + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                $('#cmbPeriodoConsult').empty();
                $("#cmbPeriodoConsult").append('<option value=0 >Seleccione un Periodo</option>');
                for (var i = 0; i <= result.length - 1; i++) {
                    $("#cmbPeriodoConsult").append('<option value=' + result[i].Periodo + '>' + result[i].Periodo + '</option>');
                }
            },
            error: function (xhr) {
                console.log(xhr.responseText);
                alert("Error has ocurred..");
            }
        });
    },
    GetDia: function () {
        $('#cmbPeriodoConsult').change(function () {
            $("#tblConsultaPedidos").dataTable().empty();

            var periodo = {
                'idCliente': $('input[name=txtidClienteConsulta]').val(),
                'Periodo': $('#cmbPeriodoConsult').val()
            };

            $.ajax({
                type: "POST",
                url: '/VentasConsultas/GetDiaClienteConsultas',
                data: "{'periodo':" + JSON.stringify(periodo) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    $('#cmbDiaConsult').empty();
                    $("#cmbDiaConsult").append('<option value=0 >Seleccione Dia</option>');
                    for (var i = 0; i <= result.length - 1; i++) {
                        $("#cmbDiaConsult").append('<option value=' + result[i].dia + '>' + result[i].dia + '</option>');
                    }
                },
                error: function (xhr) {
                    console.log(xhr.responseText);
                    alert("Error has ocurred..");
                }
            });
        });
    },
    GetFolio: function () {
        $('#cmbDiaConsult').change(function () {
            var periodo = {
                'idCliente': $('input[name=txtidClienteConsulta]').val(),
                'Periodo': $('#cmbPeriodoConsult').val(),
                'Dia': $("#cmbDiaConsult").val()
            };

            $.ajax({
                type: "POST",
                url: '/VentasConsultas/GetFolioClienteConsultas',
                data: "{'periodo':" + JSON.stringify(periodo) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    $('#cmbFolioConsult').empty();
                    $("#cmbFolioConsult").append('<option value=0 >Seleccione Folio</option>');
                    for (var i = 0; i <= result.length - 1; i++) {
                        $("#cmbFolioConsult").append('<option value=' + result[i].FolioPref + '>' + result[i].FolioPref + '</option>');
                    }
                },
                error: function (xhr) {
                    console.log(xhr.responseText);
                    alert("Error has ocurred..");
                }
            });
        });
    },
    GetPedidoBusqueda: function (IdCliente, Fecha, Prefijo, Folio, Boton, Pedido) {

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
            url: '/VentasConsultas/GetClientePedidoConsult',
            data: "{'pedido':" + JSON.stringify(pedido) + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                Consultas.ShowDataTablePedido(result);
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

            table = $("#tblConsultaPedidos").dataTable({
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
                    { title: "Factura" },
                    { title: "Complemento de Pago" },
                    { 'data': null, title: 'Pedido', wrap: true, "render": function (item) { return '<div class="btn-group"> <button type="button" id="btnFacturaPdf" onclick="Consultas.GetPedidoPDF()" value="0" class="btn btn-success btn-sm" aria-label="Left Align" data-toggle="modal" data-target="#myModal"><span class="fa fa-download"></span> Imprimir</button></div>' } },
                    { 'data': null, title: 'Abono', wrap: true, "render": function (item) { return ' <button type="button" id ="btnAbonoPdf"  onclick="Consultas.GetAbonoPDF()" value="1" class="btn btn-warning btn-sm  aria-label="Left Align" data-toggle="modal" data-target="#myModal""><span class="fa fa-download"></span>Imprimir</button>' } },

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

            $('#tblConsultaPedidos tbody').on('click', 'tr', function () {
                var data = table.api().row(this).data();

            });
        }
        else {
            $(".DTFC_Cloned tbody").empty();
            $("#tblConsultaPedidos").dataTable().empty();
        }
    },
    SearchPedido: function () {
        $('#btnPedidoConsult').click(function () {
            if ($('#txtPedidoConsult').val() != "") {
                Consultas.GetPedidoBusqueda(null, null, '', null, 1, $('#txtPedidoConsult').val());
            }
            else {
                alert('Introduzca un folio a buscar');
            }
        });
    },
    GetAbonoPDF: function () {

        $('#tblConsultaPedidos tbody').on('click', 'tr', function () {
            var data = table.api().row(this).data();
            let URLactual = window.location;
            let Url = URLactual.toString().split('/');

            let rutaReports = 'http://' + Url[2] + '/VentasRPTS/Download_AbonoVentaDormimundo_PDF?IdAbono=' + data[0] + '';
            window.location = rutaReports.replace("''", "");
        });
    },
    GetPedidoPDF: function () {
        $('#tblConsultaPedidos tbody').on('click', 'tr', function () {
            var data = table.api().row(this).data();
            let URLactual = window.location;
            let Url = URLactual.toString().split('/');

            let rutaReports = 'http://' + Url[2] + '/VentasRPTS/Download_PedidoVentaDormimundo_PDF?IDVENTA=' + data[0] + '';
            window.location = rutaReports.replace("''", "");
        });
    },
    GetPedidoxFolioPrefijo: function () {
        $('#cmbFolioConsult').change(function () {
            Consultas.GetPedidoBusqueda($('input[name=txtidClienteConsulta]').val(), $('#cmbPeriodoConsult').val(), $('#cmbFolioConsult').val().split('-')[0], $('#cmbFolioConsult').val().split('-')[1], 0, null);
        });
    },
    ClearFiltro: function () {
        $('#btnNuevoFiltro').click(function () {
            $('#txtClienteConsult').val('');
            $('#cmbPeriodoConsult').val(0);
            $('#cmbDiaConsult').val(0);
            $('#cmbFolioConsult').val(0);
            $('#txtPedidoConsult').val('');

            $("#tblConsultaPedidos").dataTable().empty();
        });
    },
    GetFacturaPDF: function (id, doc, fechaventa) {
        if (id != null) {
            if (doc == 1) {
                doc = "Factura";
            } else { doc = "Complemento"; }


            $.ajax({
                type: 'POST',
                url: '/VentasConsultas/GetCFDI',
                data: "{ 'IdAbono': '" + id + "','Documento':'" + doc + "','fechaventa':'" + fechaventa + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data == "No existe archivo") {
                        MessageError("El pdf que esta intentando accesar aun no se a creado,intentelo mas tarde ...");
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
                        MessageError("El pdf que esta intentando accesar aun no se a creado,intentelo mas tarde ...");
                        return;
                    } else {
                        window.location.href = "/VentasConsultas/Download_FactPDF?source=" + data;
                    }
                }
            });
        }
    }
    
}

$(document).ready(function () {
    Consultas.GetClientes();
    Consultas.GetDia();
    Consultas.GetFolio();
    Consultas.SearchPedido();
    Consultas.GetPedidoxFolioPrefijo();
    Consultas.ClearFiltro();
});

//ShowDataTable: function (result) {
//    var data = result;
//    if (data != null) {
//        $("#tblConsultaPedidos  tr:gt(0)").remove();
//        for (var i = 0; i <= data.length - 1; i++) {
//            var table = $("#tblConsultaPedidos > tbody");
//            table.append("<tr id='row" + i + "'>" +
//                "<td>" + data[i].id + "</td>" +
//                "<td>" + data[i].venta + "</td>" +
//                "<td>" + data[i].abono + "</td>" +
//                "<td>" + data[i].fechareciboOutput + "</td>" +
//                "<td>" + data[i].fechaventaOutput + "</td>" +
//                "<td>" + data[i].motopagado + "</td>" +
//                "<td>" + data[i].Reimprimir + "</td>" +
//                "<td>" + '<div class="btn-group"> <button type="button" id="btnFacturaPdf" onclick="Consultas.GetPedidoPDF()" value="0" class="btn btn-success" aria-label="Left Align" data-toggle="modal" data-target="#myModal">  <span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span>Imprimir</button></div>' + "</td>" +
//                "<td>" + '<div class="btn-group"> <button type="button" id ="btnAbonoPdf"  onclick="Consultas.GetAbonoPDF()" value="1" class="btn btn-warning" data-toggle="modal" data-target="#myModal">Imprimir</button></div>' + "</td>" +
//                "<td>" + '<div class="btn-group"> <button type="button" onclick="Consultas.GetFacturaPDF()" value="3" class="btn btn-warning" data-toggle="modal" data-target="#myModal">Imprimir</button></div>' + "</td>" +
//                "</tr>");
//        }
//    }
//    else {
//        $(".DTFC_Cloned tbody").empty();
//        $("#tblConsultaPedidos").dataTable().empty();
//    }
//}

function MessageError(message) {
    toastr.options.timeOut = 6000;
    toastr.warning(message);
    return;
}
function AlertNotification(title, text, icon) {
    Swal.fire(title, text, icon)
}