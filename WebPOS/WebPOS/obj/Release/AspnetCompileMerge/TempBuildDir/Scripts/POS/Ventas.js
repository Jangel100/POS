//const { type } = require("jquery");

Ventas = {
    modalBuscacliente: function () {
        $("#idbuscacliente").click(function () {
            VentaUsuario.isRequiredFacturar();
        });
    }
};

var arayJson = '';
var oTblReport = $("#tblConsultaClientesEnc");

var SelectClienteTabla;
var CPPedido = true;
VentaUsuario = {
    GetUsuario: function () {
        $("#btnBuscarCliente").click(function () {
            var txtNombre = $("#txtNombre").val();
            var txtRFC = $("#txtRFC").val();
            $('#cadClienteEncontrados').show();

            if (txtNombre == '' && txtRFC == '') {
                alert('Introduce el Nombre o el RFC para realizar la búsqueda');
            }
            else {
                var ClienteInputView = {
                    'Nombre': txtNombre.toString(),
                    'RFC': txtRFC.toString()
                };

                $.ajax({
                    type: "POST",
                    url: '/VentasUsuario/GetCliente',
                    data: "{'clienteInputView':" + JSON.stringify(ClienteInputView) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {

                        var dataset = result

                        var columnDefs = [];
                        //loop for populating column defs
                        for (let k in dataset[0]) {
                            if (columnDefs.indexOf(k) === -1) {
                                columnDefs.push({ title: dataset[0][k] });
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
                            "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                            "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                            "search": "Buscar:",
                            "infoThousands": ",",
                            "loadingRecords": "Cargando...",
                            "paginate": {
                                "first": "Primero",
                                "last": "Último",
                                "next": "Siguiente",
                                "previous": "Anterior"
                            },
                            "aria": {
                                "sortAscending": ": Activar para ordenar la columna de manera ascendente",
                                "sortDescending": ": Activar para ordenar la columna de manera descendente"
                            },
                            "buttons": {
                                "copy": "Copiar",
                                "colvis": "Visibilidad",
                                "collection": "Colección",
                                "colvisRestore": "Restaurar visibilidad",
                                "copyKeys": "Presione ctrl o u2318 + C para copiar los datos de la tabla al portapapeles del sistema. <br \/> <br \/> Para cancelar, haga clic en este mensaje o presione escape.",
                                "copySuccess": {
                                    "1": "Copiada 1 fila al portapapeles",
                                    "_": "Copiadas %d fila al portapapeles"
                                },
                                "copyTitle": "Copiar al portapapeles",
                                "csv": "CSV",
                                "excel": "Excel",
                                "pageLength": {
                                    "-1": "Mostrar todas las filas",
                                    "1": "Mostrar 1 fila",
                                    "_": "Mostrar %d filas"
                                },
                                "pdf": "PDF",
                                "print": "Imprimir"
                            },
                            "autoFill": {
                                "cancel": "Cancelar",
                                "fill": "Rellene todas las celdas con <i>%d<\/i>",
                                "fillHorizontal": "Rellenar celdas horizontalmente",
                                "fillVertical": "Rellenar celdas verticalmentemente"
                            },
                            "decimal": ",",
                            "searchBuilder": {
                                "add": "Añadir condición",
                                "button": {
                                    "0": "Constructor de búsqueda",
                                    "_": "Constructor de búsqueda (%d)"
                                },
                                "clearAll": "Borrar todo",
                                "condition": "Condición",
                                "conditions": {
                                    "date": {
                                        "after": "Despues",
                                        "before": "Antes",
                                        "between": "Entre",
                                        "empty": "Vacío",
                                        "equals": "Igual a",
                                        "notBetween": "No entre",
                                        "notEmpty": "No Vacio",
                                        "not": "Diferente de"
                                    },
                                    "number": {
                                        "between": "Entre",
                                        "empty": "Vacio",
                                        "equals": "Igual a",
                                        "gt": "Mayor a",
                                        "gte": "Mayor o igual a",
                                        "lt": "Menor que",
                                        "lte": "Menor o igual que",
                                        "notBetween": "No entre",
                                        "notEmpty": "No vacío",
                                        "not": "Diferente de"
                                    },
                                    "string": {
                                        "contains": "Contiene",
                                        "empty": "Vacío",
                                        "endsWith": "Termina en",
                                        "equals": "Igual a",
                                        "notEmpty": "No Vacio",
                                        "startsWith": "Empieza con",
                                        "not": "Diferente de"
                                    },
                                    "array": {
                                        "not": "Diferente de",
                                        "equals": "Igual",
                                        "empty": "Vacío",
                                        "contains": "Contiene",
                                        "notEmpty": "No Vacío",
                                        "without": "Sin"
                                    }
                                },
                                "data": "Data",
                                "deleteTitle": "Eliminar regla de filtrado",
                                "leftTitle": "Criterios anulados",
                                "logicAnd": "Y",
                                "logicOr": "O",
                                "rightTitle": "Criterios de sangría",
                                "title": {
                                    "0": "Constructor de búsqueda",
                                    "_": "Constructor de búsqueda (%d)"
                                },
                                "value": "Valor"
                            },
                            "searchPanes": {
                                "clearMessage": "Borrar todo",
                                "collapse": {
                                    "0": "Paneles de búsqueda",
                                    "_": "Paneles de búsqueda (%d)"
                                },
                                "count": "{total}",
                                "countFiltered": "{shown} ({total})",
                                "emptyPanes": "Sin paneles de búsqueda",
                                "loadMessage": "Cargando paneles de búsqueda",
                                "title": "Filtros Activos - %d"
                            },
                            "select": {
                                "1": "%d fila seleccionada",
                                "_": "%d filas seleccionadas",
                                "cells": {
                                    "1": "1 celda seleccionada",
                                    "_": "$d celdas seleccionadas"
                                },
                                "columns": {
                                    "1": "1 columna seleccionada",
                                    "_": "%d columnas seleccionadas"
                                }
                            },
                            "thousands": ".",
                            "datetime": {
                                "previous": "Anterior",
                                "next": "Proximo",
                                "hours": "Horas",
                                "minutes": "Minutos",
                                "seconds": "Segundos",
                                "unknown": "-",
                                "amPm": [
                                    "am",
                                    "pm"
                                ]
                            },
                            "editor": {
                                "close": "Cerrar",
                                "create": {
                                    "button": "Nuevo",
                                    "title": "Crear Nuevo Registro",
                                    "submit": "Crear"
                                },
                                "edit": {
                                    "button": "Editar",
                                    "title": "Editar Registro",
                                    "submit": "Actualizar"
                                },
                                "remove": {
                                    "button": "Eliminar",
                                    "title": "Eliminar Registro",
                                    "submit": "Eliminar",
                                    "confirm": {
                                        "_": "¿Está seguro que desea eliminar %d filas?",
                                        "1": "¿Está seguro que desea eliminar 1 fila?"
                                    }
                                },
                                "error": {
                                    "system": "Ha ocurrido un error en el sistema (<a target=\"\\\" rel=\"\\ nofollow\" href=\"\\\">Más información&lt;\\\/a&gt;).<\/a>"
                                },
                                "multi": {
                                    "title": "Múltiples Valores",
                                    "info": "Los elementos seleccionados contienen diferentes valores para este registro. Para editar y establecer todos los elementos de este registro con el mismo valor, hacer click o tap aquí, de lo contrario conservarán sus valores individuales.",
                                    "restore": "Deshacer Cambios",
                                    "noMulti": "Este registro puede ser editado individualmente, pero no como parte de un grupo."
                                }
                            },
                            "info": "Mostrando de _START_ a _END_ de _TOTAL_ entradas"
                        }

                        var table = $("#tblConsultaClientesEnc").dataTable({
                            destroy: true,
                            language: spanish,
                            scrollX: true,
                            scrollCollapse: true,
                            fixedColumns: true,
                            select: true,
                            data: data,
                            columns: [
                                { title: "Id" },
                                { title: "Nombre" },
                                { title: "RFC" },
                                { title: "Calle/Numero" },
                                { title: "Colonia" },
                                { title: "Telefono" }
                            ],

                            "columnDefs": [
                                { "searchable": true, "orderable": true, targets: "_all" },
                                { "className": "text-center custom-middle-align", targets: "_all" }

                            ],
                            "processing": true,
                            "pageLength": 10,
                            "lengthChange": false,
                            "responsive": true,
                            "rowReorder": {
                                "selector": 'td:nth-child(2)'
                            },
                            "fixedHeader": {
                                "header": true
                            },
                            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                            lengthChange: false,
                            responsive: true,
                            autoWidth: false,
                            oLanguage: {
                                oPaginate: {
                                    sNext: 'Siguiente<span class= "glyphicon glyphicon-chevron-left"></span>',
                                    sPrevious: '<span class= "glyphicon glyphicon-chevron-right"></span>Anterior'
                                }
                            },
                            iDisplayLength: 5,
                            responsive: {
                                pagingType: "simple"
                            }
                        });

                        $('#tblConsultaClientesEnc tbody').on('click', 'tr', function () {
                            var data = table.api().row(this).data();

                            VentaUsuario.showClientes(data[0], data[2]);
                        });

                        ///* Add a click handler for the delete row */
                        //$('#btnEliminarEncontradosSeleted').click(function () {
                        //    var tableNew = $("#tblConsultaClientesEnc").dataTable();
                        //    tableNew.find('tbody tr.selected').remove();
                        //    $(".DTFC_Cloned tbody").empty();
                        //});
                    },
                    error: function (xhr) {
                        console.log(xhr.responseText);
                        alert("Error has ocurred..");
                    }
                });
            }
        });
    },
    OnlClickRowConsultaClientes: function (table) {
        table.on('select', function (e, dt, type, indexes) {
            if (type === 'row') {
                var data = table.api().rows(indexes).data().pluck('id');

                alert(data);
            }
        });
    },
    showClientes: function (idcliente, RFC) {
        var ClienteInputView = {
            'Id': idcliente.toString(),
            'RFC': RFC
        };

        $.ajax({
            type: "POST",
            url: '/VentasUsuario/GetClienteFacturacion',
            data: "{'clienteInputView':" + JSON.stringify(ClienteInputView) + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var ls = result;

                $('#collapseTwo').removeClass("show");
                $('#collapseOne').addClass("show");

                //******Se limpia los valores de Docimilio al realizar una nueva busqueda******//
                $("#txtCalle").val('');
                $("#txtNumeroInt").val('');
                $("#txtColonia").val('');
                $("#txtNumeroExt").val('');
                $("#txtCiudad").val('');
                $("#txtCP").val('');
                $("#txtEstados").val('');
                $("#txtCalleFact").val('');
                $("#txtNumeroIntFact").val('');
                $("#txtColoniaFact").val('');
                $("#txtNumeroExtFact").val('');
                $("#txtCiudadFact").val('');
                $("#txtCPFact").val('');
                $("#txtEstadosFact").val('');
                //VentaUsuario.enabledTxtApellidos();
                //******Se limpia los valores de Docimilio al realizar una nueva busqueda******//

                if (ls.ClientesFacturacion != null) {
                    VentaUsuario.loadDatosPedidos(ls.Clientes);
                    VentaUsuario.GetCPFacturacion(ls.Clientes.actCod_COMUNAColonia, ls.Clientes.CP, ls.ClientesFacturacion.CP, "");
                    VentaUsuario.SearchCPLoadN(ls.Clientes.actCod_COMUNAColonia, "");

                    VentaUsuario.loadDatosClienteFacturacion(ls.ClientesFacturacion);
                    VentaUsuario.GetCPFacturacionFact(ls.ClientesFacturacion.actCod_COMUNAcolonia, ls.ClientesFacturacion.CP, ls.Clientes.CP, "Fact");
                    //VentaUsuario.SearchCPLoadN(ls.ClientesFacturacion.actCod_COMUNAcolonia, "Fact");

                    if ($('input[name=optionRequiredFact]:checked').val() == 2) {
                        VentaUsuario.GetCPFacturacionFact(ls.ClientesFacturacion.actCod_COMUNAcolonia, ls.ClientesFacturacion.CP, ls.Clientes.CP, "Fact");
                        VentaUsuario.SearchCPLoadN(ls.ClientesFacturacion.actCod_COMUNAcolonia, "Fact");
                    }
                }
                else {
                    VentaUsuario.loadDatosPedidos(ls.Clientes);
                    VentaUsuario.GetCP(ls.Clientes.actCod_COMUNAColonia, "");
                }
                $('#cadClienteEncontrados').hide();
            },
            error: function (xhr) {
                console.log(xhr.responseText);
                alert("Error has ocurred..");
            }
        }).done(function () {
            $("#txtCP").blur();
            $("#txtCPFact").blur();
            //$("input[name=txtCodColoniaFact]").val(0);
        });
    },
    loadDatosPedidos: function (Clientes) {
        // $('input[name=txtIdCliente]').val(Clientes.Id);
        if (Clientes.actNOMBRE === null) { $("#txtName").val(""); }
        else
            $("#txtName").val(Clientes.actNOMBRE);
        $("#txtAPaterno").val(Clientes.actAPEPATERNO);
        $("#txtAMaterno").val(Clientes.actAPEMATERNO);
        if (Clientes.RFC.toString().indexOf(' ') != -1 ||
            Clientes.RFC.toString().length < 11 ||
            Clientes.RFC.toString().length > 13) {
            $("#txtRFCN").val('XAXX010101000');
        }
        else {
            $("#txtRFCN").val(Clientes.RFC);
        }
        if (Clientes.actCALLE === null) { $("#txtCalle").val() }
        else
            $("#txtCalle").val(Clientes.actCALLE);
        if (Clientes.actNUMINT === null) { $("#txtNumeroInt").val(""); }
        else
            $("#txtNumeroInt").val(Clientes.actNUMINT);
        $("#txtColonia").val(Clientes.Colonia);
        $("#txtNumeroExt").val(Clientes.actNUMEXT);
        $("#txtDelegacion").val(Clientes.DelMun);
        $("#txtCP").val(Clientes.CP);
        $("#txtCorreo").val(Clientes.Correo);

        $("#txtTelCasa").val(Clientes.TelCasa);
        $("#txtTelOfc").val(Clientes.TelOfc);
        $("#txtNoCelular").val(Clientes.NoCelular);

        $("#txtEstados").val(Clientes.actCod_REGIONestado);

        var now = moment().format('DD-MM-YYYY');
        var momentDate = moment(Clientes.actFCHNACIMIENTO).format("DD-MM-YYYY");

        if (now == momentDate) {
            $("#txFechaNacimiento").val('');
        }
        else
            $("#txFechaNacimiento").val(momentDate);
    },
    loadDatosClienteFacturacion: function (clientesF) {
        $('input[name=txtIdCliente]').val(clientesF.Id);
        $("#txtCalleFact").val(clientesF.actCALLE);
        $("#txtNumeroIntFact").val(clientesF.actNUMINT);
        $("#txtColoniaFact").val(clientesF.Colonia);
        $("#txtNumeroExtFact").val(clientesF.actNUMEXT);
        $("#txtDelegacionFact").val(clientesF.DelMun);
        $("#txtCPFact").val(clientesF.CP);

        if (clientesF.actCod_REGIONestado != null)
            $("#txtEstadosFact").val(clientesF.Estado);

        //var now = moment().format('DD-MM-YYYY');
        //var momentDate = moment(clientesF.actFCHNACIMIENTO).format("DD-MM-YYYY");

        //if (now == momentDate) {
        //    $("#txFechaNacimiento").val('');
        //}
        //else
        //    $("#txFechaNacimiento").val(momentDate);

    },
    DeleteClientesEncontrados: function () {
        $("#btnEliminarEncontrados").click(function () {

            $(".DTFC_Cloned tbody").empty();
            $("#tblConsultaClientesEnc").dataTable().empty();
            $("#tblConsultaClientesEnc_paginate").hide();
        });
    },
    SelectedDatosIguales: function () {
        $("#chkCheckDatosIguales").change(function () {
            if ($(this).prop('checked')) {
                let txtcalle = $("#txtCalle").val();
                let txtNumeroInt = $("#txtNumeroInt").val();
                let txtColonia = $("#cmbColonia").val();
                let txtNumeroExt = $("#txtNumeroExt").val();
                let txtDelegacion = $("#txtMunicipios").val();
                let txtCiudad = $("#txtCiudad").val();
                let txtCP = $("#txtCP").val();
                let cmbEstados = $("#txtEstados").val();

                $("#txtCalleFact").val(txtcalle);
                $("#txtNumeroIntFact").val(txtNumeroInt);
                $("#txtNumeroExtFact").val(txtNumeroExt);
                $("#txtMunicipiosFact").val(txtDelegacion);
                $("#txtCPFact").val(txtCP);
                VentaUsuario.SearchCPLoad(txtColonia, 'Fact');
                $("#cmbColoniaFact").val(txtColonia);
                $("#txtCiudadFact").val(txtCiudad);

            }
        });
    },
    CapturarCliente: function () {
        $("#btnCapturarCliente").click(function () {

            let validar = true;

            validar = VentaUsuario.Validateform("Registrar");

            if (validar == true) {

                if ($('input[name=optionRequiredFact]:checked').val() == 2) {
                    $('#txtRFCN').prop('required', true);
                    if ($("#txtRFCN").val() == "") {
                        $("#toastClientes .toast-body").html("<p>Agregue el campo RFC </p>");

                        $("#toastClientes").toast('show');

                        return false;
                    }
                }

                let txtName = $("#txtName").val();
                let txtAPaterno = $("#txtAPaterno").val();
                let txtAMaterno = $("#txtAMaterno").val();
                let txtRFCN = $("#txtRFCN").val();
                let txtCorreo = $("#txtCorreo").val();
                let txFechaNacimiento = $("#txFechaNacimiento").val();
                let txtcalle = $("#txtCalle").val();
                let txtNumeroInt = $("#txtNumeroInt").val();
                let txtColonia = $("#cmbColonia option:selected").text();
                let actCod_COMUNAcolonia = $("#cmbColonia").val();
                let txtNumeroExt = $("#txtNumeroExt").val();
                let txtDelegacion = $("#txtMunicipios").val();
                let txtCP = $("#txtCP").val();
                let cmbEstados = $("#txtEstados").val();

                let txtcalleFact = $("#txtCalleFact").val();
                let txtNumeroExtFact = $("#txtNumeroExtFact").val();
                let txtNumeroIntFact = $("#txtNumeroIntFact").val();
                let txtColoniaFact = $("#cmbColoniaFact option:selected").text();
                let actCod_COMUNAcoloniaFact = $("#cmbColoniaFact").val();
                let txtDelegacionFact = $("#txtMunicipiosFact").val();
                let txtCPFact = $("#txtCPFact").val();
                let cmbEstadosFact = $("#txtEstadosFact").val();

                let txtTelCasa = $("#txtTelCasa").val();
                let txtTelOfc = $("#txtTelOfc").val();
                let txtNoCelular = $("#txtNoCelular").val();

                if (txtRFCN == '') {
                    txtRFCN = 'XAXX010101000'
                }

                let IDstore = -1;
                let Store = sessionStorage.getItem('storeId');  //Se obtiene el valor de session de idStore en localstorage

                if (Store == null) {
                    IDstore = 0;
                }
                else {
                    IDstore = parseInt(sessionStorage.getItem('storeId').toString());
                }

                let cliente = {
                    "IdStore": IDstore,
                    "Nombre": txtName.toString() + " " + txtAPaterno.toString() + " " + txtAMaterno.toString(),
                    "RFC": txtRFCN.toString(),
                    "CalleNumeroFact": txtcalle.toString(),
                    "actNOMBRE": txtName.toString(),
                    "actAPEPATERNO": txtAPaterno.toString(),
                    "actAPEMATERNO": txtAMaterno.toString(),
                    "actNUMEXTFact": txtNumeroExt.toString(),
                    "actNUMINTFact": txtNumeroInt.toString(),
                    "ColoniaFact": txtColonia.toString(),
                    "actCod_COMUNAColoniaFact": actCod_COMUNAcolonia.toString(),
                    "CPFact": txtCP.toString(),
                    "Correo": txtCorreo.toString(),
                    "actCod_REGIONestadoFact": cmbEstados.toString(),
                    "DelMunFact": txtDelegacion.toString(),
                    "TipoCliente": "C",

                    "actFCHNACIMIENTOTemp": txFechaNacimiento.toString(),
                    "CalleNumero": txtcalleFact.toString(),
                    "actNUMEXT": txtNumeroExtFact.toString(),
                    "actNUMINT": txtNumeroIntFact.toString(),
                    "Colonia": txtColoniaFact.toString(),
                    "CP": txtCPFact.toString(),
                    "actCod_REGIONestado": cmbEstadosFact.toString(),
                    "actCod_COMUNAcolonia": actCod_COMUNAcoloniaFact.toString(),
                    "DelMun": txtDelegacionFact.toString(),
                    "TelCasa": txtTelCasa.toString(),
                    "TelOfc": txtTelOfc.toString(),
                    "NoCelular": txtNoCelular.toString()
                };

                $.ajax({
                    type: "POST",
                    url: '/VentasUsuario/GetInsertInfoCliente',
                    data: "{'clientes':" + JSON.stringify(cliente) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result == '-1') {
                            alert('El RFC que usted esta dando de alta ya se encuentra registrado favor de agregar otro diferente');
                        }
                        else {
                            if (result == '3') {
                                alert('No se realizo el registro del cliente');
                            }
                            else {
                                alert('Se Registro el Cliente Nuevo ' + result);
                                VentaUsuario.dataSeleccionarCliente(result);
                            }
                        }

                        if (result === '5') {
                            alert('Verifique que todos los datos esten llenos');
                        }

                    },
                    error: function (xhr) {
                        console.log(xhr.responseText);
                        alert("Error has ocurred..");
                    }
                });
            }
        });
    },
    Validateform: function (Operacion) {
        $('#toastClientes').toast({ delay: 8000 });

        $('#txtName').attr("required", "required");
        $("#txtCorreo").attr("required", "required");
        $("#txtCalle").attr("required", "required");
        $("#txtNumeroInt").attr("maxlength", "200");
        $("#cmbColonia").attr("required", "required");
        $("#txtNumeroExt").attr("required", "required");
        $("#txtNumeroExt").attr("maxlength", "200");
        $("#txtMunicipios").attr("required", "required");
        $("#txtMunicipios").attr("maxlength", "100");
        $("#txtCP").attr("required", "required");
        $("#txtCP").attr("maxlength", "20");
        $("#txtTelCasa").attr("maxlength", "20");
        $("#txtTelOfc").attr("maxlength", "20");
        $("#txtNoCelular").attr("maxlength", "12");

        let fecha = $("#txFechaNacimiento").val();

        if (Operacion != "ActualizarFact")
            if (Operacion != "Seleccionar") {
                if (fecha != null && fecha != "") {

                    let isValida = validarFecha(fecha);

                    if (!isValida) {

                        isValida = validateDate(fecha);
                        if (!isValida) {

                            $("#toastClientes .toast-body").html("<p>Introduzca una fecha de nacimiento correcta</p>");
                            $("#toastClientes").toast('show');

                            return false;
                        }
                    }
                }
            }

        //if (Operacion == "ActualizarFact") {
        //    if ($('#txtCorreo').val() == "") {
        //        $("#toastClientes .toast-body").html("Es necesario un correo eléctronico para seleccionar un cliente");

        //        $("#toastClientes").toast('show');
        //        ;
        //        $('#collapseTwo').addClass("show");
        //        $('#collapseOne').addClass("show");
        //    }
        //    if ($('#cmbColonia').val() == "-1") {
        //        $("#toastClientes .toast-body").html("Es necesario agregar el nombre de colonia para el envio a domicilio");

        //        $("#toastClientes").toast('show');
        //    }

        //    if ($('input[name=optionRequiredFact]:checked').val() == 2) {
        //        if ($('#txtCPFact').val() == "") {
        //            $("#toastClientes .toast-body").html("Es necesario agregar el Código Postal para realizar la facturación");

        //            $("#toastClientes").toast('show');

        //            return false;
        //        }
        //        if ($('#cmbColoniaFact').val() == "-1") {
        //            $("#toastClientes .toast-body").html("Es necesario agregar el nombre de colonia para realizar la facturación");

        //            $("#toastClientes").toast('show');

        //            return false;
        //        }
        //        if ($('#txtCalleFact').val() == "") {
        //            $("#toastClientes .toast-body").html("Es necesario agregar el nombre de la calle para realizar la facturación");

        //            $("#toastClientes").toast('show');

        //            return false;
        //        }
        //        if ($('#txtNumeroExtFact').val() == "") {
        //            $("#toastClientes .toast-body").html("Es necesario agregar el numero exterior para realizar la facturacion");

        //            $("#toastClientes").toast('show');

        //            return false;
        //        }
        //    }
        //}
        $("#frmClientes input").each(function () {
            var elemento = this;
            //if ($("#cmboTipoPersona").val() == "Fisica") {
            //    if ($("#txtAPaterno").val() == "") {
            //        $("#toastClientes .toast-body").html("<p>La persona Fisica requiere el campo: Apellido Paterno</p>");

            //        $("#toastClientes").toast('show');
            //        return false;
            //    }
            //    if ($("#txtAMaterno").val() == "") {
            //        $("#toastClientes .toast-body").html("<p>La persona Fisica requiere el campo:Apellido Materno</p>");

            //        $("#toastClientes").toast('show');
            //        return false;
            //    }
            //}

            if (Operacion == "Actualizar" ||
                Operacion == "ActualizarFact"
                && elemento.id == "txtRFCN") {
                if (elemento.value === "" && elemento.getAttribute("name") != null) {
                    if (elemento.id == "txtCalle" ||
                        elemento.id == "txtNumeroExt" ||
                        elemento.id == "txtCP") {
                        $("#toastClientes .toast-body").html("<p>Agregue el Campo de dirección de entrega de pedido: " + elemento.getAttribute("name") + "</p>");
                        $("#toastClientes").toast('show');
                    }
                }
            } else {
                if (elemento.id == "txtCPFact" ||
                    elemento.id == "cmbColoniaFact" ||
                    elemento.id == "txtNumeroExtFact") {
                    if ($('input[name=optionRequiredFact]:checked').val() == 2) {
                        if (elemento.value === "" && elemento.getAttribute("name") != null) {
                            $("#toastClientes .toast-body").html("<p>Agregue el Campo: " + elemento.getAttribute("name") + "</p>");

                            $("#toastClientes").toast('show');
                            ;
                            $('#collapseTwo').addClass("show");
                            $('#collapseOne').addClass("show");

                            return false;
                        }
                    }
                } else {
                    if (elemento.id != "txtIdCliente" &&
                        elemento.id != "txtEstadosFact" &&
                        elemento.id != "txtMunicipiosFact" &&
                        elemento.id != "txtCalleFact" &&
                        elemento.id != "txtNumeroIntFact" &&
                        elemento.id != "txtNumeroExtFact" &&
                        elemento.id != "txtTelCasa" &&
                        elemento.id != "txtTelOfc" &&
                        elemento.id != "txtNoCelular" &&
                        elemento.id != "txtAPaterno" &&
                        elemento.id != "txtAMaterno" &&
                        elemento.id != "txFechaNacimiento" &&
                        elemento.id != "txtNumeroInt") {
                        if (elemento.value === "" && elemento.getAttribute("name") != null) {

                            if (elemento.id == "txtCalle" ||
                                elemento.id == "txtNumeroExt" ||
                                elemento.id == "txtCP") {
                                $("#toastClientes .toast-body").html("<p>Agregue el Campo de dirección de entrega de pedido: " + elemento.getAttribute("name") + "</p>");
                                $("#toastClientes").toast('show');
                            }
                            else {
                                if ($('input[name=optionRequiredFact]:checked').val() == 2 && $("#txtRFCN").val() == "") {
                                    $("#toastClientes .toast-body").html("<p>Agregue el Campo RFC para realizar la factura(Realiza la atualizacion del cliente para guardar sus datos  de facturación por favor y despues selecciona el cliente)</p>");
                                    $("#toastClientes").toast('show');
                                }
                                if (elemento.id != "txtRFCN") {
                                    $("#toastClientes .toast-body").html("<p>Agregue el Campo: " + elemento.getAttribute("name") + "</p>");
                                    $("#toastClientes").toast('show');
                                }
                            }
                        }
                    }
                }
            }
        });

        if ($('input[name=optionRequiredFact]:checked').val() == 2) {
            $("#txtCalleFact").attr("maxlength", "100");
            $("#txtCalleFact").attr("required", "required");
            $("#txtNumeroExtFact").attr("maxlength", "200");
            $("#txtNumeroExtFact").attr("required", "required");
            $("#txtNumeroIntFact").attr("maxlength", "200");
            $("#txtCPFact").attr("required", "required");
            $("#txtEstadosFact").attr("required", "required");

            if ($('#cmbColoniaFact').val() == "-1") {
                $("#toastClientes .toast-body").html("Es necesario agregar el nombre de colonia para realizar la facturación(Realiza la atualizacion del cliente para guardar sus datos  de facturación por favor y despues selecciona el cliente)");

                $("#toastClientes").toast('show');

                return false;
            }
            if ($('#txtCalleFact').val() == "") {
                $("#toastClientes .toast-body").html("Es necesario agregar el nombre de la calle para realizar la facturación(Realiza la atualizacion del cliente para guardar sus datos  de facturación por favor y despues selecciona el cliente)");

                $("#toastClientes").toast('show');

                return false;
            }
            if ($('#txtNumeroExtFact').val() == "") {
                $("#toastClientes .toast-body").html("Es necesario agregar el numero exterior para realizar la facturacion(Realiza la atualizacion del cliente para guardar sus datos  de facturación por favor y despues selecciona el cliente)");

                $("#toastClientes").toast('show');

                return false;
            }
        }

        $('#frmClientes').attr('novalidate');
        let form = $("#frmClientes")

        let validar = true;

        if (form[0].checkValidity() === false) {
            event.preventDefault()
            event.stopPropagation()

            validar = false;
        }

        if ($('#cmbColonia').val() == -1 && validar == true) {
            $("#toastClientes .toast-body").html("<p>Agregue el Campo: Colonia</p>");
            $("#toastClientes").toast('show');
            return false;
        }

        if (form[0].checkValidity() === true && validar == true) {
            return validar;
        }
    },
    EventClick: function () {

        var form = document.forms["frmClientesAdd"];

        for (var i = 0; i <= form.length - 1; i++) {
            if (form[i].value.length > 1) {
                $("#" + form[i].id + "").css({
                    'border': '1px solid #ced4da'
                });
            }
        }
    },
    CapturarClienteN: function () {
        $("#btnCapturarAdd").click(function () {
            var validar = true;

            $('#toastClientesAdd').toast({ delay: 8000 });

            $('#txtNameAdd').attr("required", "required");
            $("#txtAPaternoAdd").attr("required", "required");
            $("#txtAMaternoAdd").attr("required", "required");
            $("#txtRFCN").attr("required", "required");
            $("#txtRFCN").attr("maxlength", "13");
            $("#txtCorreoAdd").attr("required", "required");
            $("#txtCalleAdd").attr("required", "required");
            $("#txtNumeroIntAdd").attr("required", "required");
            $("#txtNumeroIntAdd").attr("maxlength", "6");
            $("#cmbColoniaAdd").attr("required", "required");
            $("#txtNumeroExtAdd").attr("required", "required");
            $("#txtNumeroExtAdd").attr("maxlength", "6");
            $("#txtMunicipiosAdd").attr("required", "required");
            $("#txtMunicipiosAdd").attr("maxlength", "100");
            $("#txtCPAdd").attr("required", "required");
            $("#txtCPAdd").attr("maxlength", "20");
            $("#txtTelCasaAdd").attr("maxlength", "20");
            $("#txtTelOficinaAdd").attr("maxlength", "20");
            $("#cmbColoniaAdd").attr("required", "required");
            $("#txtNumCelAdd").attr("maxlength", "12");
            $('#frmClientesAdd').attr('novalidate');


            var form = document.forms["frmClientesAdd"];

            $("#frmClientesAdd input").each(function () {
                var elemento = this;
                if (elemento.id != "rfcaltainp" &&
                    elemento.id != "txtEstadosAdd" &&
                    elemento.id != "txtMunicipiosAdd" &&
                    elemento.id != "txtCiudadAdd" &&
                    elemento.id != "txtTelCasaAdd" &&
                    elemento.id != "txtTelOficinaAdd" &&
                    elemento.id != "txtNumCelAdd") {
                    if (elemento.value === "" && elemento.getAttribute("name") != null) {
                        $("#toastClientesAdd .toast-body").html("<p>Agregue el Campo: " + elemento.getAttribute("name") + "</p>");

                        $("#toastClientesAdd").toast('show');
                        validar = false;
                        return false;
                    }
                }
            });

            if ($('input[name=optionRequiredFact]:checked').val() == 1) {
                if (form[0].checkValidity() === false) {
                    event.preventDefault()
                    event.stopPropagation();

                    validar = false;
                }
                else {
                    validar = true;
                }
                if ($("#rfcaltainp").val() == "") {
                    event.preventDefault()
                    event.stopPropagation();

                    validar = false;

                    $("#toastClientesAdd .toast-body").html("<p>Introduzca RFC</p>");
                    $("#toastClientesAdd").toast('show');
                    return false;
                } else {
                    validar = true;
                }

                if ($('#tipopersona').val() == '') {
                    validar = false

                    $("#toastClientesAdd .toast-body").html("<p>Introduzca el tipo de persona</p>");
                    $("#toastClientesAdd").toast('show');
                }
            }

            if (validar === false) {
                return false;
            }

            //Validacion de Correo
            var expresion = /^[-\w.%+]{1,64}@(?:[A-Z0-9-]{1,63}\.){1,125}[A-Z]{2,63}$/i;
            var email = form[15].value;
            if (!expresion.test($('#txtCorreoAdd').val())) {
                $("#toastClientesAdd .toast-body").html("<p>Introduzca un email correcto</p>");

                $("#toastClientesAdd").toast('show');
                return false;
            }

            // Validacion de Colonia
            if ($("#cmbColoniaAdd").val() == -1 && validar == true) {
                $("#toastClientesAdd .toast-body").html("<p>Falta por seleccionar una colonia en direccíon de entrega de pedido</p>");

                $("#toastClientesAdd").toast('show');
                return false;
            }

            //Validacion de Fechas
            var fecha = $("#txFechaNacimientoAdd").val();

            //if (VentaUsuario.isValidDate(fecha) == false) {
            //    $("#toastClientesAdd .toast-body").html("<p>Cambie la Fecha de Nacimiento por una valida </p>");
            //    $("#toastClientesAdd").toast('show');

            //    return false;
            //}

            if (fecha != null && fecha != "") {
                // Create a new moment object
                var now = moment().format('MMM-DD-YYYY');

                var FechaNacimiento = moment(fecha).format('MMM-DD-YYYY');

                var a = moment(fecha);
                var b = moment(now);
                var dif = a.diff(b) // 

                //Validacion Fecha Mayor
                //if (dif > 0) {
                //    $("#toastClientesAdd .toast-body").html("<p>Cambie la Fecha de Nacimiento por una valida </p>");
                //    $("#toastClientesAdd").toast('show');

                //    return false;
                //}

                //Validacion Fecha igual
                //if (now == FechaNacimiento) {
                //    $("#toastClientesAdd .toast-body").html("<p>Cambie la Fecha de Nacimiento por una valida: </p>");
                //    $("#toastClientesAdd").toast('show');

                //    return false;
                //}
            }
            else {
                $("#toastClientesAdd .toast-body").html("<p>Cambie la Fecha de Nacimiento por una valida: </p>");
                $("#toastClientesAdd").toast('show');

                return false;
            }

            if (form[0].checkValidity() === true && validar == true) {
                var txtName = $("#txtNameAdd").val();
                var txtAPaterno = $("#txtAPaternoAdd").val();
                var txtAMaterno = $("#txtAMaternoAdd").val();
                var txtRFCN = $("#rfcaltainp").val();
                var txtCorreo = $("#txtCorreoAdd").val();
                var txFechaNacimiento = $("#txFechaNacimientoAdd").val();
                var txtcalle = $("#txtCalleAdd").val();
                var txtNumeroInt = $("#txtNumeroIntAdd").val();
                var txtColonia = $("#cmbColoniaAdd option:selected").text();
                var actCod_COMUNAcolonia = $("#cmbColoniaAdd").val();
                var txtNumeroExt = $("#txtNumeroExtAdd").val();
                var txtDelegacion = $("#txtMunicipiosAdd").val();
                var txtCP = $("#txtCPAdd").val();
                var cmbEstados = $("#txtEstadosAdd").val();

                var txtcalleFact = $("#txtCalleAdd").val();
                var txtNumeroExtFact = $("#txtNumeroExtAdd").val();
                var txtNumeroIntFact = $("#txtNumeroIntAdd").val();
                var txtColoniaFact = $("#cmbColoniaAdd option:selected").text();
                var actCod_COMUNAcoloniaFact = $("#cmbColoniaAdd").val();
                var txtDelegacionFact = $("#txtMunicipiosAdd").val();
                var txtCPFact = $("#txtCPAdd").val();
                var cmbEstadosFact = $("#txtEstadosAdd").val();

                var txtTelCasa = $("#txtTelCasaAdd").val();
                var txtTelOfc = $("#txtTelOficinaAdd").val();
                var txtNoCelular = $("#txtNumCelAdd").val();

                var cliente = {
                    "Nombre": txtName.toString() + " " + txtAPaterno.toString() + " " + txtAMaterno.toString(),
                    "RFC": txtRFCN.toString(),
                    "CalleNumero": txtcalle.toString(),
                    "actNOMBRE": txtName.toString(),
                    "actAPEPATERNO": txtAPaterno.toString(),
                    "actAPEMATERNO": txtAMaterno.toString(),
                    "actNUMEXT": txtNumeroExt.toString(),
                    "actNUMINT": txtNumeroInt.toString(),
                    "Colonia": txtColonia.toString(),
                    "actCod_COMUNAColonia": actCod_COMUNAcolonia.toString(),
                    "CP": txtCP.toString(),
                    "Correo": txtCorreo.toString(),
                    "actCod_REGIONestado": cmbEstados.toString(),
                    "DelMun": txtDelegacion.toString(),
                    "TipoCliente": "C",

                    "actFCHNACIMIENTOTemp": txFechaNacimiento.toString(),
                    "CalleNumeroFact": txtcalleFact.toString(),
                    "actNUMEXTFact": txtNumeroExtFact.toString(),
                    "actNUMINTFact": txtNumeroIntFact.toString(),
                    "ColoniaFact": txtColoniaFact.toString(),
                    "CPFact": txtCPFact.toString(),
                    "actCod_REGIONestadoFact": cmbEstadosFact.toString(),
                    "actCod_COMUNAcoloniaFact": actCod_COMUNAcoloniaFact.toString(),
                    "DelMunFact": txtDelegacionFact.toString(),
                    "TelCasa": txtTelCasa.toString(),
                    "TelOfc": txtTelOfc.toString(),
                    "NoCelular": txtNoCelular.toString()
                };

                $.ajax({
                    type: "POST",
                    url: '/VentasUsuario/GetInsertInfoClienteNew',
                    data: "{'clientes':" + JSON.stringify(cliente) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {

                        if (result === -2) {
                            alert('Ocurrio un error al registrar el cliente');
                        }

                        if (result === 5) {
                            alert('Verifique que todos los datos esten llenos');
                            return false;
                        }


                        if (result == -1) {
                            alert('No se realizo el registro, Favor de verificar la FechaNacimiento/RFC');
                            return false;
                        }
                        else {
                            alert('Se registro el cliente ' + result);

                        }
                        var x = document.getElementById("contenidobusqueda");
                        var y = document.getElementById("altacliente");
                        if (x.style.display === "none") {
                            y.style.display = "none";
                            x.style.display = "block";

                        } else {

                        }

                        $("#BuscaclienteModal").modal('hide');
                    },
                    error: function (xhr) {
                        console.log(xhr.responseText);
                        alert("Error has ocurred..");
                    }
                });
            }
        });
    },
    ActualizarClienteN: function () {
        $("#btnActualizar").click(function () {
            $("#txtRFCN").removeAttr("required");
            var validate = VentaUsuario.Validateform("Actualizar");
            if (validate == true) {
                var txtIdCliente = $("input[name=txtIdCliente]").val();
                var txtName = $("#txtName").val();
                var txtAPaterno = $("#txtAPaterno").val();
                var txtAMaterno = $("#txtAMaterno").val();
                var txtRFCN = $("#txtRFCN").val();
                var txtCorreo = $("#txtCorreo").val();
                var txFechaNacimiento = $("#txFechaNacimiento").val();
                var txtcalleFact = $("#txtCalle").val();
                var txtNumeroInt = $("#txtNumeroIntFact").val();
                var txtColoniaFact = $("#cmbColonia option:selected").text();
                var actCod_COMUNAcoloniaFact = $("#cmbColonia").val();
                var txtNumeroExt = $("#txtNumeroExtFact").val();
                var txtDelegacionFact = $("#txtMunicipios").val();
                var txtCPFact = $("#txtCP").val();
                var cmbEstadosFact = $("#txtEstados").val();
                var txtcalle = $("#txtCalleFact").val();
                var txtNumeroExtFact = $("#txtNumeroExt").val();
                var txtNumeroIntFact = $("#txtNumeroInt").val();
                var txtColonia = $("#cmbColoniaFact option:selected").text();
                var actCod_COMUNAcolonia = $("#cmbColoniaFact").val();
                var txtDelegacion = $("#txtMunicipiosFact").val();
                var txtCP = $("#txtCPFact").val();
                var cmbEstados = $("#txtEstadosFact").val();

                var txtTelCasa = $("#txtTelCasa").val();
                var txtTelOfc = $("#txtTelOfc").val();
                var txtNoCelular = $("#txtNoCelular").val();
                let IdStore = sessionStorage.getItem('storeId');

                var cliente = {
                    "Id": txtIdCliente.toString(),
                    "IdStore": IdStore.toString(),
                    "Nombre": txtName.toString(),
                    "RFC": txtRFCN.toString(),
                    "CalleNumero": txtcalle.toString(),
                    "actNOMBRE": txtName.toString(),
                    "actAPEPATERNO": txtAPaterno.toString(),
                    "actAPEMATERNO": txtAMaterno.toString(),
                    "actNUMEXT": txtNumeroExt.toString(),
                    "actNUMINT": txtNumeroInt.toString(),
                    "Colonia": txtColonia.toString(),
                    "CP": txtCP.toString(),
                    "Correo": txtCorreo.toString(),
                    "actCod_REGIONestado": cmbEstados.toString(),
                    "DelMun": txtDelegacion.toString(),
                    "TipoCliente": "C",
                    "TelCasa": txtTelCasa.toString(),
                    "TelOfc": txtTelOfc.toString(),
                    "NoCelular": txtNoCelular.toString(),
                    "actFCHNACIMIENTOTemp": txFechaNacimiento.toString(),
                    "CalleNumeroFact": txtcalleFact.toString(),
                    "actNUMEXTFact": txtNumeroExtFact.toString(),
                    "actNUMINTFact": txtNumeroIntFact.toString(),
                    "ColoniaFact": txtColoniaFact.toString(),
                    "CPFact": txtCPFact.toString(),
                    "actCod_REGIONestadoFact": cmbEstados.toString(),
                    "DelMunFact": txtDelegacionFact.toString(),
                    "actCod_REGIONestadoFact": cmbEstadosFact.toString(),
                    "actCod_COMUNAcoloniaFact": actCod_COMUNAcoloniaFact.toString(),
                    "actCod_COMUNAcolonia": actCod_COMUNAcolonia.toString(),
                };
                $.ajax({
                    type: "POST",
                    url: '/VentasUsuario/GetUpdateInfoCliente',
                    data: "{'clientes':" + JSON.stringify(cliente) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {

                        if (result === "3" || (result === "1" || result > 3)) {
                            alert('Se realizo la actualización del cliente');

                            // $("#BuscaclienteModal").modal('hide');
                        }
                        else {
                            alert('No Se actualizo el cliente ');
                        }
                    },
                    error: function (xhr) {
                        console.log(xhr.responseText);
                        alert("Error has ocurred..");
                    }
                });
            }
        });
    },
    SeleccionarCliente: function () {
        $("#btnSeleccionar").click(function () {
            let txtIdCliente = $('input[name=txtIdCliente]').val();

            let IsValidate = false;

            IsValidate = VentaUsuario.Validateform("ActualizarFact");

            if (IsValidate == true) {
                VentaUsuario.ActualizarFactClienteN(); //Actualiza Direccion de Facturacion en caso de que no tenga
                VentaUsuario.dataSeleccionarCliente(txtIdCliente);
            }
        });
    },
    ClearBtnclienteAdd: function () {
        $('#btnClearclienteAdd').click(function () {
            VentaUsuario.CleanAddCliente();
        });
    },
    CloseModalCliente: function () {
        $("#BuscaclienteModal").on("hidden.bs.modal", function () {
            VentaUsuario.ClearCliente();
        });
    },
    CleanAddCliente: function () {
        $("#frmClientesAdd")[0].reset();
        $("#checkrfc").prop('checked', false);
        $("#tipopersona").val(-1);
        $("#tipopersona").attr("disabled", "disabled");
        $("#rfcalta").hide();
        $("#rfcaltainp").hide();

        $("#frmClientesAdd input").each(function () {
            var elemento = this;
            $("#" + elemento.id + "").removeAttr("required");
        });
    },
    CloseModalClienteNew: function () {
        $("#BuscaclienteModal").on("hidden.bs.modal", function () {
            VentaUsuario.clearClient();
            $('#cadClienteEncontrados').hide();
        });
    },
    ClearCliente: function () {
        $("#frmClientes")[0].reset();
        $("#cmbColonia").val(-1);
        $("#cmbColoniaFact").val(-1);

        $("#frmClientes input").each(function () {
            var elemento = this;
            $("#" + elemento.id + "").removeAttr("required");
        });
    },
    ClearDatosCliente: function () {
        $('#btnClearDatosClientes').click(function () {
            VentaUsuario.ClearCliente();
        });
    },
    ClonarVenta: function () {
        $('#btnClonarVenta').click(function () {
            $('#modalClonVenta').modal('show');
        });
    },
    SearchVentaC: function () {
        $('#btnbusquedaFolioClon').click(function () {
            var Sale = {
                'Folio': $('#ClFolio').val(),
                'Fecha': $('#ClFecha').val()
            }

            if ($('#ClFolio').val() == '' && $('#ClFecha').val() == '') {
                alert('Ingrese Folio/Fecha para realizar la busqueda');

                return false;
            }

            $.ajax({
                type: "POST",
                url: '/Ventas/GetSearchVenta',
                data: "{'Sale':" + JSON.stringify(Sale) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    var dataset = result

                    if (dataset != null) {
                        var columnDefs = [];
                        //loop for populating column defs
                        for (let k in dataset[0]) {
                            if (columnDefs.indexOf(k) === -1) {
                                columnDefs.push({ title: dataset[0][k] });
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

                        table = $("#tblFoliosEncontradosC").dataTable({
                            destroy: true,
                            language: spanish,
                            fixedColumns: true,
                            select: true,
                            data: data,
                            "processing": true,
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

                        $('#tblFoliosEncontradosC tbody').on('click', 'tr', function () {
                            var IdVenta = table.api().row(this).data()[0];

                            var Sale = {
                                'ID': IdVenta
                            }

                            $.ajax({
                                type: "POST",
                                url: '/Ventas/GetSearchDetalleVenta',
                                data: "{'Sale':" + JSON.stringify(Sale) + "}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (result) {

                                    var data = result;
                                    var j = 0;
                                    $("#tbBody  tr:gt(0)").remove();
                                    for (var i = 0; i <= data.lsDetailsVenta.length - 1; i++) {
                                        var table = $("#tbBody > tbody");
                                        table.append("<tr id='row" + i + "'>" +
                                            "<td>" + data.lsDetailsVenta[i].Modelo + "</td>" +
                                            "<td hidden='" + "hidden" + "'>" + i + "</td>" + "" +
                                            "<td>" + data.lsDetailsVenta[i].Juego + "</td>" +
                                            "<td>" + data.lsDetailsVenta[i].Cantidad + "</td>" +
                                            "<td>" + data.lsDetailsVenta[i].Tienda + "</td>" +
                                            "<td>" + data.lsDetailsVenta[i].Bodega + "</td>" +
                                            "<td>" + data.lsDetailsVenta[i].Lista + "</td>" +
                                            "<td>$" + data.lsDetailsVenta[i].Unitario + "</td>" +
                                            "<td>$" + data.lsDetailsVenta[i].Descuento + "</td>" +
                                            "<td>$" + data.lsDetailsVenta[i].Subtotal + "</td>" +
                                            "<td>$" + data.lsDetailsVenta[i].IVA + "</td>" +
                                            "<td class='cTotal'>$" + data.lsDetailsVenta[i].Total + "</td>" +
                                            "<td>" + data.lsDetailsVenta[i].Itemcode + "</td>" +
                                            "<td><button type='button' name='remove' id='" + i + "' class='btn btn-danger btn-sm btn_remove1'>" +
                                            "<i class='fa fa-trash' aria-hidden='true'></i> Eliminar</button></td>" +
                                            "</tr>");
                                    }

                                    if (data.lsDetailsAbonosVenta != null) {
                                        $("#tblCPagosEnc tr:gt(0)").remove();

                                        var x = 0;
                                        for (var j = 0; j <= data.lsDetailsAbonosVenta.length - 1; j++) {
                                            var tbl = $("tblCPagosEnc");
                                            x = x + 1;

                                            var contendor = $("#tblCPagosEnc").html();
                                            var row = "<tr  id='rowp" + x + "'>";
                                            row += "<td>" + data.lsDetailsAbonosVenta[j].FormaPago + "</td>";
                                            row += "<td>$" + data.lsDetailsAbonosVenta[j].Monto + "</td>";
                                            row += "<td>" + data.lsDetailsAbonosVenta[j].NoCuenta + "</td>";
                                            row += "<td>" + data.lsDetailsAbonosVenta[j].TipoTarjeta + "</td>";
                                            row += "<td>" + data.lsDetailsAbonosVenta[j].MSI + "</td>";
                                            row += "<td><button type='button' name='remove' id='" + x + "' class='btn btn-danger btn-sm btn_remopago'><i class='fa fa-trash' aria-hidden='true'></i>Eliminar</button></td>";
                                            row += "</tr>"
                                            $("#tblCPagosEnc").html(contendor + row);
                                        }
                                    }

                                    CalculaTotal();

                                    $("#modalClonVenta").modal("hide");;
                                },
                                error: function (xhr) {
                                    console.log(xhr.responseText);
                                    alert("Error has ocurred..");
                                }
                            });

                        });
                    }
                    else {
                        $(".DTFC_Cloned tbody").empty();
                        $("#tblFoliosEncontradosC").dataTable().empty();
                    }
                },
                error: function (xhr) {
                    console.log(xhr.responseText);
                    alert("Error has ocurred..");
                }
            });
        });
    },
    SearchCPLoad: function (c_colonia, Tipo) {
        if ($("#txtCP" + Tipo + "").val().length > 5 && $("#txtCP" + Tipo + "").val().length < 5) {
        }
        else {
            var CPSepomex = {
                'd_codigo': $('#txtCP' + Tipo + '').val()
            }
            $.ajax({
                url: "/VentasUsuario/GetInfoCP",
                type: "POST",
                data: "{'CPSepomex':" + JSON.stringify(CPSepomex) + "}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.length > 0) {
                        $('#txtEstados' + Tipo + '').val(data[0].d_estado);
                        $('#txtMunicipios' + Tipo + '').val(data[0].d_mnpio);
                        $('#txtCiudad' + Tipo + '').val(data[0].d_ciudad);
                        $('#cmbColonia' + Tipo + '').html('');
                        $('#cmbColonia' + Tipo + '').append("<option value='-1'>Seleccione Colonia</option>");

                        for (var i = 0; i <= data.length - 1; i++) {

                            if (c_colonia == data[i].c_colonia && c_colonia != null) {
                                $('#cmbColonia' + Tipo + '').append("<option value=" + data[i].c_colonia + " selected >" + data[i].d_asenta + "</option>");
                            }
                            else {
                                $('#cmbColonia' + Tipo + '').append("<option value=" + data[i].c_colonia + ">" + data[i].d_asenta + "</option>");
                            }
                        }
                    }
                    else {
                        if (c_colonia != "" && c_colonia > 0 || $("#txtCP" + Tipo + "").val() != "") {
                            if (Tipo == "") {
                                $("#modalCPVenta").modal('show');
                            }
                            if (Tipo == "Fact") {
                                $("#modalCPVentaFact").modal('show');
                            }
                            if (Tipo == "Add") {
                                $("#modalCPVentaAdd").modal('show');
                            }
                        }
                    }
                }
            });
        }
    },
    SearchCPLoadN: function (c_colonia, Tipo) {
        var CPSepomex = {
            'd_codigo': $('#txtCP' + Tipo + '').val()
        }
        $.ajax({
            url: "/VentasUsuario/GetInfoCP",
            type: "POST",
            data: "{'CPSepomex':" + JSON.stringify(CPSepomex) + "}",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.length > 0) {
                    $('#txtEstados' + Tipo + '').val(data[0].d_estado);
                    $('#txtMunicipios' + Tipo + '').val(data[0].d_mnpio);
                    $('#txtCiudad' + Tipo + '').val(data[0].d_ciudad);
                    $('#cmbColonia' + Tipo + '').html('');
                    $('#cmbColonia' + Tipo + '').append("<option value='-1'>Seleccione Colonia</option>");

                    for (var i = 0; i <= data.length - 1; i++) {

                        if (c_colonia == data[i].c_colonia && c_colonia != null) {
                            $('#cmbColonia' + Tipo + '').append("<option value=" + data[i].c_colonia + " selected >" + data[i].d_asenta + "</option>");
                        }
                        else {
                            $('#cmbColonia' + Tipo + '').append("<option value=" + data[i].c_colonia + ">" + data[i].d_asenta + "</option>");
                        }
                    }
                }
            }
        });
    },
    ValidarCP: function (Tipo) {
        VentaUsuario.SearchCPLoad("", Tipo);
    },
    GetCPFacturacion: function (Colonia, CP, CPFact, Tipo) {

        var CPSepomex = {
            'd_codigo': CP
        }
        $.ajax({
            url: "/VentasUsuario/GetInfoCP",
            type: "POST",
            data: "{'CPSepomex':" + JSON.stringify(CPSepomex) + "}",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.length > 0) {

                    CPPedido = true;

                    $('#txtEstados' + Tipo + '').val(data[0].d_estado);
                    $('#txtMunicipios' + Tipo + '').val(data[0].d_mnpio);
                    $('#txtCiudad' + Tipo + '').val(data[0].d_ciudad);
                    $('#cmbColonia' + Tipo + '').html('');
                    $('#cmbColonia' + Tipo + '').append("<option value='-1'>Seleccione Colonia</option>");

                    for (var i = 0; i <= data.length - 1; i++) {

                        if (Colonia == data[i].c_colonia && Colonia != null) {
                            $('#cmbColonia' + Tipo + '').append("<option value=" + data[i].c_colonia + " selected >" + data[i].d_asenta + "</option>");
                        }
                        else {
                            $('#cmbColonia' + Tipo + '').append("<option value=" + data[i].c_colonia + ">" + data[i].d_asenta + "</option>");
                        }
                    }
                }
                else {
                    CPPedido = false;
                    $("input[name=txtCodColoniaFact]").val(CPFact);
                    if (Tipo == "") {
                        $("#modalCPVenta").modal('show');
                    }
                    if (Tipo == "Fact") {
                        $("#modalCPVentaFact").modal('show');
                    }
                    if (Tipo == "Add") {
                        $("#modalCPVentaAdd").modal('show');
                    }
                }
            }
        });
    },
    GetCPFacturacionFact: function (Colonia, CP, CPFact, Tipo) {

        var CPSepomex = {
            'd_codigo': CP
        }
        $.ajax({
            url: "/VentasUsuario/GetInfoCP",
            type: "POST",
            data: "{'CPSepomex':" + JSON.stringify(CPSepomex) + "}",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.length > 0) {
                    $('#txtEstados' + Tipo + '').val(data[0].d_estado);
                    $('#txtMunicipios' + Tipo + '').val(data[0].d_mnpio);
                    $('#txtCiudad' + Tipo + '').val(data[0].d_ciudad);
                    $('#cmbColonia' + Tipo + '').html('');
                    $('#cmbColonia' + Tipo + '').append("<option value='-1'>Seleccione Colonia</option>");

                    for (var i = 0; i <= data.length - 1; i++) {

                        if (Colonia == data[i].c_colonia && Colonia != null) {
                            $('#cmbColonia' + Tipo + '').append("<option value=" + data[i].c_colonia + " selected >" + data[i].d_asenta + "</option>");
                        }
                        else {
                            $('#cmbColonia' + Tipo + '').append("<option value=" + data[i].c_colonia + ">" + data[i].d_asenta + "</option>");
                        }
                    }
                }
                else {

                    var CPSepomex = {
                        'd_codigo': CP
                    }

                    $.ajax({
                        url: "/VentasUsuario/GetInfoCP",
                        type: "POST",
                        data: "{'CPSepomex':" + JSON.stringify(CPSepomex) + "}",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.length > 0) {
                                CPPedido = true;
                            }
                            else {
                                CPPedido = false;
                            }
                        }
                    });
                    if (CPPedido == true) {
                        if (Tipo == "Fact") {
                            $("#modalCPVentaFact").modal('show');
                        }
                    }
                }
            }
        });
    },
    GetCP: function (Colonia, Tipo) {
        VentaUsuario.SearchCPLoad(Colonia, Tipo);
    },
    GetCPEncontrados: function (Tipo) {
        var CPSepomex = {
            'd_asenta': $("#txtd_asentamiento" + Tipo + "").val()
        }
        $.ajax({
            url: "/VentasUsuario/GetInfoCPAsentamiento",
            type: "POST",
            data: "{'CPSepomex':" + JSON.stringify(CPSepomex) + "}",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var dataset = data

                if (dataset != null) {
                    var columnDefs = [];
                    //loop for populating column defs
                    for (let k in dataset[0]) {
                        if (columnDefs.indexOf(k) === -1) {
                            columnDefs.push({ title: dataset[0][k] });
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

                    table = $("#tblCPEncontrados" + Tipo + "").dataTable({
                        destroy: true,
                        language: spanish,
                        fixedColumns: true,
                        select: true,
                        data: data,
                        "processing": true,
                        "pageLength": 10,
                        "lengthChange": false,
                        "scrollY": "200px",
                        "scrollx": "200px",
                        "scrollCollapse": true,
                        "responsive": true,
                        "rowReorder": {
                            "selector": 'td:nth-child(2)'
                        },
                        "fixedHeader": {
                            "header": true
                        }
                    });

                    $("#tblCPEncontrados" + Tipo + " tbody").on('click', 'tr', function () {
                        let colonia = table.api().row(this).data()[0];
                        let CP = table.api().row(this).data()[1];

                        VentaUsuario.SearchCPNew(CP, colonia, Tipo);

                        $("#txtCP" + Tipo + "").val(CP);
                        $("#tblCPEncontrados" + Tipo + "").dataTable().empty();
                        $("#modalCPVenta" + Tipo + "").modal("hide");
                        if ($("#txtCiudadFact").val() == "") {
                            CPPedido = true
                        }

                        if (CPPedido == true) {
                            if ($("input[name=txtCodColoniaFact]").val() > 0) {
                                $("#modalCPVentaFact").modal("show");
                            }
                        }

                        $("input[name=txtCodColoniaFact]").val(0);
                        $("#txtd_asentamiento" + Tipo + "").val('');
                        $("#txtd_asentamientoFact").val('');
                        $('#BuscaclienteModal').css('overflow', 'auto')

                    });
                }
                else {
                    $(".DTFC_Cloned tbody").empty();
                    $("#txtd_asentamiento" + Tipo + "").val('');
                    $("#tblCPEncontradosC").dataTable().empty();
                }
            }
        });
    },
    SearchCPNew: function (CP, Colonia, Tipo) {
        var CPSepomex = {
            'd_codigo': CP
        }

        $.ajax({
            url: "/VentasUsuario/GetInfoCP",
            type: "POST",
            data: "{'CPSepomex':" + JSON.stringify(CPSepomex) + "}",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.length > 0) {
                    $("#txtEstados" + Tipo + "").val(data[0].d_estado);
                    $("#txtMunicipios" + Tipo + "").val(data[0].d_mnpio);
                    $("#txtCiudad" + Tipo + "").val(data[0].d_ciudad);
                    $("#cmbColonia" + Tipo + "").html('');
                    $("#cmbColonia" + Tipo + "").append("<option value='-1'>Seleccione Colonia</option>");

                    for (var i = 0; i <= data.length - 1; i++) {
                        if (Colonia == data[i].c_colonia && Colonia != null) {
                            $('#cmbColonia' + Tipo + '').append("<option value=" + data[i].c_colonia + " selected >" + data[i].d_asenta + "</option>");
                        }
                        else {
                            $('#cmbColonia' + Tipo + '').append("<option value=" + data[i].c_colonia + ">" + data[i].d_asenta + "</option>");
                        }
                    }
                }
                else {
                    $("#modalCPVenta").modal('show');
                }
                $('#BuscaclienteModal').css('overflow', 'auto')
            }
        });
    },
    GetCPEncontradosFact: function (Tipo) {
        var CPSepomex = {
            'd_asenta': $("#txtd_asentamiento" + Tipo + "").val()
        }
        $.ajax({
            url: "/VentasUsuario/GetInfoCPAsentamiento",
            type: "POST",
            data: "{'CPSepomex':" + JSON.stringify(CPSepomex) + "}",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var dataset = data

                if (dataset != null) {
                    var columnDefs = [];
                    //loop for populating column defs
                    for (let k in dataset[0]) {
                        if (columnDefs.indexOf(k) === -1) {
                            columnDefs.push({ title: dataset[0][k] });
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

                    table = $("#tblCPEncontrados" + Tipo + "").dataTable({
                        destroy: true,
                        language: spanish,
                        fixedColumns: true,
                        select: true,
                        data: data,
                        "processing": true,
                        "lengthChange": false,
                        "scrollY": "200px",
                        "scrollx": "200px",
                        "scrollCollapse": true,
                        "responsive": true,
                        "rowReorder": {
                            "selector": 'td:nth-child(2)'
                        },
                        "fixedHeader": {
                            "header": true
                        },
                        info: false,
                        lengthChange: false,
                        responsive: true,
                        autoWidth: false,
                        oLanguage: {
                            oPaginate: {
                                sNext: 'Siguiente <i class="fas fa-angle-double-right"></i>',
                                sPrevious: '<i class="fas fa-angle-double-left"></i> Previos'
                            }
                        },
                        iDisplayLength: 5,
                        pagingType: "simple_numbers"
                    });

                    $("#tblCPEncontrados" + Tipo + " tbody").on('click', 'tr', function () {
                        let colonia = table.api().row(this).data()[0];
                        let CP = table.api().row(this).data()[1];

                        VentaUsuario.SearchCPNew(CP, colonia, Tipo);

                        $("#txtd_asentamiento" + Tipo + "").val('');
                        $("#txtCP" + Tipo + "").val(CP);
                        $("#modalCPVenta" + Tipo + "").modal("hide");
                        $("#BuscaclienteModal").addClass();

                        $("#tblCPEncontrados" + Tipo + "").dataTable().empty();
                        $("input[name=txtCodColoniaFact]").val(0);
                        $("#txtd_asentamientoFact").val('');
                        $('#BuscaclienteModal').css('overflow', 'auto')
                    });
                }
                else {
                    $(".DTFC_Cloned tbody").empty();
                    $("#txtd_asentamiento" + Tipo + "").val('');
                    $("#tblCPEncontrados" + Tipo + "").dataTable().empty();
                    $('#BuscaclienteModal').css('overflow', 'auto')
                }
            }
        });
    },
    SolonumerosCP: function () {
        // Listen for the input event.
        $("#txtCP").on('input', function (evt) {
            // Allow only numbers.
            jQuery(this).val(jQuery(this).val().replace(/[^0-9]/g, ''));
        });

        // Listen for the input event.
        $("#txtCPFact").on('input', function (evt) {
            // Allow only numbers.
            jQuery(this).val(jQuery(this).val().replace(/[^0-9]/g, ''));
        });

        // Listen for the input event.
        $("#txtCPAdd").on('input', function (evt) {
            // Allow only numbers.
            jQuery(this).val(jQuery(this).val().replace(/[^0-9]/g, ''));
        });
    },
    isValidDate: function (dateString) {

        // convertir los numeros a enteros
        var parts = dateString.split("-");
        var day = parseInt(parts[2], 10);
        var month = parseInt(parts[1], 10);
        var year = parseInt(parts[0], 10);

        // Revisar los rangos de año y mes
        if ((year < 1900) || (year > 3000) || (month == 0) || (month > 12))
            return false;

        var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        // Revisar el rango del dia
        return day > 0 && day <= monthLength[month - 1];
    },
    CloseValidacionCP: function () {
        $('#BuscaclienteModal').css('overflow', 'auto');
    },
    SolonumerosyLetrasNombre: function () {
        // Listen for the input event.
        //$("#txtName").on('input', function (evt) {
        //    jQuery(this).val(jQuery(this).val().replace(/[^A-Za-zÁÉÍÓÚáéíóúñÑ-9 ]+$/g, ''));
        //});
        $("#txtName").on('input', function (evt) {
            // Allow only numbers.
            jQuery(this).val(jQuery(this).val().replace(/[^A-Za-zÁÉÍÓÚáéíóúñÑ0-9 ]+$/g, ''));
        });
        $("#txtNameAdd").on('input', function (evt) {
            // Allow only numbers.
            jQuery(this).val(jQuery(this).val().replace(/[^A-Za-zÁÉÍÓÚáéíóúñÑ ]+$/g, ''));
        });

        $("#txtAPaterno").on('input', function (evt) {
            // Allow only numbers.
            jQuery(this).val(jQuery(this).val().replace(/[^A-Za-zÁÉÍÓÚáéíóúñÑ0-9 ]+$/g, ''));
        });
        $("#txtAPaternoAdd").on('input', function (evt) {
            // Allow only numbers.
            jQuery(this).val(jQuery(this).val().replace(/[^A-Za-zÁÉÍÓÚáéíóúñÑ ]+$/g, ''));
        });
        $("#txtAMaterno").on('input', function (evt) {
            // Allow only numbers.
            jQuery(this).val(jQuery(this).val().replace(/[^A-Za-zÁÉÍÓÚáéíóúñÑ0-9 ]+$/g, ''));
        });
        $("#txtAMaternoAdd").on('input', function (evt) {
            // Allow only numbers.
            jQuery(this).val(jQuery(this).val().replace(/[^A-Za-zÁÉÍÓÚáéíóúñÑ ]+$/g, ''));
        });
    },
    SoloNumeroLetrasRFC: function () {
        $("#txtRFCN").on('input', function (evt) {
            // Allow only numbers.
            jQuery(this).val(jQuery(this).val().replace(/[^A-Za-zÁÉÍÓÚáéíóúñÑ0-9 ]+$/g, ''));
        });
        $("#rfcaltainp").on('input', function (evt) {
            // Allow only numbers.
            jQuery(this).val(jQuery(this).val().replace(/[^A-Za-zÁÉÍÓÚáéíóúñÑ0-9 ]+$/g, ''));
        });
    },
    closeModalCPAdd: function () {
        $('#BuscaclienteModal').css('overflow', 'auto')
    },
    removeTextcheck: function () {
        $(".cssCheckFact").find(".switch-on").text("Si");
        $(".cssCheckFact").find(".switch-off").text("No");
    },
    modifyCheckAdd: () => {

        $(".cssCheckAdd").find(".switch-on").html("<i class='fas fa-plus'></i>");
        $(".cssCheckAdd").find(".switch-on").text("Agregar");
        $(".cssCheckAdd").find(".switch-off").text("Buscar");
        $(".cssCheckAdd").find(".switch-off").removeClass("btn-light");
        $(".cssCheckAdd").find(".switch-off").addClass("btn btn-info");
        $(".cssCheckAdd").find(".switch").css("width", "85px");
    },
    onchangeCheckAdd: () => {
        $('#btnActualizar').hide();
        $('#btnSeleccionar').hide();
        $("#inlineCheckAgregar").change(() => {
            if ($("#inlineCheckAgregar").prop('checked')) {
                $('#txtRFC').focus();
                $("#cmboTipoPersona").removeAttr("disabled");
                $("#txtRFC").attr("disabled", true);
                $("#txtNombre").attr("disabled", true);
                $("#btnBuscarCliente").attr("disabled", true);
                VentaUsuario.clearClient();
                $('#btnActualizar').hide();
                $('#btnSeleccionar').hide();
                $("#btnCapturarCliente").show();
                document.getElementById("txtName").focus();
                $(".cssCheckAdd").find(".switch-off").addClass("btn btn-info");
                $('.cardBusquedaCliente').hide();
            }
            else {
                $("#txtRFC").removeAttr("disabled");
                $('#txtName').focus();
                $("#txtNombre").removeAttr("disabled");
                $("#btnBuscarCliente").removeAttr("disabled");
                $("#txtRFCN").val('');
                $("#checkrfc").prop("checked", false);
                $('#btnActualizar').show();
                $('#btnSeleccionar').show();
                $("#btnCapturarCliente").hide();
                $('.cardBusquedaCliente').show();

                document.getElementById("txtRFC").focus();
            }
        });
    },
    disabledCheckRfc: () => {
        $("#checkrfc").attr("disabled", true);
    },
    actionCheck: () => {
        $("#cmboTipoPersona").attr("disabled", true);
        $('input[name=optionRequiredFact]').change(() => {
            if ($('input[name=optionRequiredFact]:checked').val() == 2) {
                $("#txtRFCN").removeAttr("disabled");
                $("#txtRFCN").val('');
                $("#cmboTipoPersona").removeAttr("disabled");
                $("#txtRFCN").val("");
                $("#txtRFCN").removeAttr('maxlength');
                $("#txtRFCN").attr('maxlength', 13);
                //VentaUsuario.disableTxtApellidos();

                //$("#cmbFormaPago").removeAttr("disabled");
                //$("#cmbUsoCFDI").removeAttr("disabled");

                //document.getElementById('inlineCheckFacturar').switchButton('on', true);
            }
            else {
                document.getElementById("txtRFC").focus();
                $("#txtRFCN").attr("disabled", true);
                $("#txtRFCN").val("");
                $("#cmboTipoPersona").attr("disabled", true);
                $("#txtRFCN").val("");
                $("#txtRFCN").removeAttr('maxlength');
                $("#txtRFCN").attr('maxlength', 12);
                //VentaUsuario.disableTxtApellidos();

                //$("#cmbFormaPago").prop("disabled", "disabled");
                //$("#cmbUsoCFDI").prop("disabled", "disabled");
                //document.getElementById('inlineCheckFacturar').switchButton('off', true);
            }
        });
    },
    clearClient: () => {
        $('#txtRFC').val('');
        $('#txtNombre').val('');
        $(".DTFC_Cloned tbody").empty();
        $("#tblConsultaClientesEnc").dataTable().empty();
        VentaUsuario.ClearCliente();
    },
    dataSeleccionarCliente: (txtIdCliente) => {
        let txtName = $('#txtName').val();
        let txtAPaterno = $('#txtAPaterno').val();
        let txtAMaterno = $('#txtAMaterno').val();
        let txtRFCN = $('#txtRFCN').val();
        let txtCalle = $('#txtCalle').val();
        let txtCorreoCliente = $('#txtCorreo').val();

        $('input[name=txtIdClienteIndex]').val(txtIdCliente);
        $('#nameV').val(txtName + ' ' + txtAPaterno + ' ' + txtAMaterno);
        $('#rfcV').val(txtRFCN);
        $('#direccionV').val(txtCalle + ' ' + $('#txtNumeroExt').val() + ' ' + $("#cmbColonia option:selected").text() + ' ' + $('#txtMunicipios').val());
        $('#direccionF').val($('#txtCalleFact').val() + ' ' + $('#txtNumeroExtFact').val() + ' ' + $("#cmbColoniaFact option:selected").text() + ' ' + $('#txtMunicipiosFact').val());
        $("#BuscaclienteModal").modal('hide');
        $("#tblConsultaClientesEnc_paginate").hide();
        $('input[name=txtCorreocliente]').val(txtCorreoCliente);
    },
    modifyCheckRFC: () => {
        $(".cssCheckFact").find(".switch-on").text("Si");
        $(".cssCheckFact").find(".switch-off").text("No");
    },
    showModalCliente: () => {
        $('#BuscaclienteModal').on('show.bs.modal', function (e) {
            setTimeout(function () {
                $('#txtRFC').focus();
            }, 500);

            VentaUsuario.modifyCheckRFC();
            VentaUsuario.modifyCheckAdd();
            VentaUsuario.removeTextcheck();
            VentaUsuario.SolonumerosyLetrasNombre();
            //VentaUsuario.disableTxtApellidos();

            $('#cadClienteEncontrados').hide();
            $('.cardBusquedaCliente').hide();
        })
    },
    onchangeTipoPersona: () => {
        $("#cmboTipoPersona").change(() => {
            if ($("#cmboTipoPersona").val() == "Fisica") {
                $("#lbNombreP").html('Nombre');
                $("#lbRFCP").html('RFC P. Fisica');
                $("#txtRFCN").val('');
                $("#txtRFCN").removeAttr("maxlength");
                $("#txtRFCN").attr("maxlength", 13);
                VentaUsuario.disableTxtApellidos();
            } else {
                $("#lbRFCP").html('RFC P. Moral');
                $("#lbNombreP").html('R. Social');
                $("#txtRFCN").val('');
                $("#txtRFCN").removeAttr("maxlength");
                $("#txtRFCN").attr("maxlength", 12);
                VentaUsuario.enabledTxtApellidos();
            }
        });
    },
    disableTxtApellidos: () => {
        $("#txtAPaterno").removeAttr('disabled');
        $("#txtAMaterno").removeAttr('disabled');
    },
    enabledTxtApellidos: () => {
        $("#txtAPaterno").attr('disabled', true);
        $("#txtAMaterno").attr('disabled', true);
    },
    disabledTipoPersona: () => {
        $("#cmboTipoPersona").val("Fisica");
        if ($("#cmboTipoPersona").val() == "Fisica") {
            $("#cmboTipoPersona").attr('disabled', true);
        }
        else {
            $("#cmboTipoPersona").removeAttr('disabled', true);
        }
    },
    loadDataListFacturacion: () => {
        $('#ConfirmarventaModal').on('show.bs.modal', () => {
            let valTipoVenta = $('input[name=optionRequiredTipoVenta]:checked').val();

            $.ajax({
                url: "/Ventas/GetDataListFacturacion",
                type: "POST",
                data: "{}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    let data = response;
                    if (data != null) {
                        let lsUsoCFDI = data.lsUsoCFDI;
                        let lsMetodoPagos = data.lsMetodoPagos;

                        if (lsUsoCFDI != null) {
                            $('#cmbMetodoPago').empty();
                            $('#cmbMetodoPago').append("<option value ='-1'>Seleccione metodo de pago</option>");
                            for (let item in lsMetodoPagos) {
                                $('#cmbMetodoPago').append("<option value ='" + lsMetodoPagos[item].MetodoPago + "'>" + lsMetodoPagos[item].Descripcion + "</option>");
                            }
                        }
                        if (lsMetodoPagos != null) {
                            $('#cmbUsoCFDI').empty();
                            $('#cmbUsoCFDI').append("<option value ='-1'>Seleccione uso CFDI</option>");
                            for (let item in lsUsoCFDI) {
                                $('#cmbUsoCFDI').append("<option value ='" + lsUsoCFDI[item].UsoCFDI + "'>" + lsUsoCFDI[item].Descripcion + "</option>");
                            }
                        }

                        let txtCorreocliente = $('input[name=txtCorreocliente]').val();

                        //***Set Correo Cliente y Correo Vendedor
                        $('#txtEmailCliente').val(txtCorreocliente);
                        $('input[name=txtEmailUsuario]').val(data.CorreoUsuario);

                        $('#cmbMetodoPago').val(valTipoVenta).change();
                    }
                }
            });
        });
    },
    loadDataListFormaPago: (FormaPago, idFormaPago) => {
        let valMetodoPago = FormaPago;
        let valFormaPago = idFormaPago;

        let metodoPago = {
            "IdMetodoPago": valMetodoPago.toString(),
            "FormaPago": valFormaPago.toString()
        }

        $.ajax({
            url: "/Ventas/GetDataListFormaPagoFacturacion",
            type: "POST",
            data: "{'metodoPago':" + JSON.stringify(metodoPago) + "}",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                let data = response;
                if (data != null) {
                    $('#cmbFormaPago').empty();
                    $('#cmbFormaPago').append("<option value ='-1'>Seleccione forma de pago</option>");
                    for (let item in data) {
                        $('#cmbFormaPago').append("<option value ='" + data[item].FormaDePago + "'>" + data[item].Descripcion + "</option>");
                    }
                    $("#cmbFormaPago").val(1);
                }
            }
        });
    },
    disabledSelectFacturacion: () => {
        $('#inlineCheckFacturar').change(() => {
            if ($('#inlineCheckFacturar').prop('checked')) {
                $('#cmbFormaPago').removeAttr('disabled');
                $('#cmbUsoCFDI').removeAttr('disabled');
            }
            else {
                $('#cmbFormaPago').prop('disabled', 'disabled');
                $('#cmbUsoCFDI').prop('disabled', 'disabled');
            }
        });
    },
    validaRequiredFactura: () => {
        let valradio1 = $('input[name=optionRequiredFact]:checked').val();

        return valradio1;
    },
    getFormaPago: (formapago) => {
        let nameFormaPago = '';

        switch (formapago) {
            case "1":
                nameFormaPago = 'Efectivo';
                break;
            case "2":
                nameFormaPago = 'Terminal Banamex';
                break;
            case "3":
                nameFormaPago = 'Terminal Bancomer';
                break;
            case "4":
                nameFormaPago = 'American Express';
                break;
            case "5":
                nameFormaPago = 'Terminal PROSA';
                break;
            case "6":
                nameFormaPago = 'Terminal Santander';
                break;
            case "7":
                nameFormaPago = 'Deposito / Cheque';
                break;
            case "8":
                nameFormaPago = 'Transferencia Electronica';
                break;
            case "9":
                nameFormaPago = 'Monedero Payback';
                break;
            case "10":
                nameFormaPago = 'Compensación';
                break;
        }
        return nameFormaPago;
    },
    getNumberFormaPago: (formapago) => {
        let FormaPago = 0;
        switch (formapago) {
            case "Efectivo":
                FormaPago = 1;
                break;
            case "Deposito / Cheque":
                FormaPago = 7;
                break;
            case "Transferencia Electronica":
                FormaPago = 8;
                break;
            case "Monedero Payback":
                FormaPago = 9;
                break;
            case "Compensación":
                FormaPago = 10;
                break;
            default:
                FormaPago = formapago;
                break;
        }
        return FormaPago;
    },
    getTipoTarjeta: () => {
        let tipoTarjeta = $('input[name=txtTipoTarjeta]').val();
        if (tipoTarjeta != '-1') {
            switch (tipoTarjeta) {
                case "Credito":
                    FormaPago = 4;
                    break;
                case "Debito":
                    FormaPago = 28;
                    break;
            }
            return FormaPago;
        }
    },
    showTabVentasSiguienteClientes: () => {
        $('#btnTabSiguienteClientes').click(() => {
            var pasa = VentaUsuario.requiredTipoFactura();
            if (pasa == 'true') {
                $('.nav-tabs a[href="#articulos-tab"]').tab('show');
                document.getElementById("datalistmodelos").focus();
            }
            let name = $('#nameV').val();
            if ($('input[name=optionRequiredFact]:checked').val() == 2) {
                if (name.indexOf("CLIENTEMOSTRADOR") == 0) {
                    $('#optionRequiredFact').focus();
                    MessageError("Usted eligio que el ''cliente requiere factura'', en este momento esta el cliente ''CLIENTEMOSTRADOR'',  favor de seleccionar un cliente y con los datos de facturación");
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return true;
            }
        });
    },
    showTabActive: () => {
        $('.nav-tabs a[href="#clientes-tab-i"]').tab('show');
    },
    showTabArticulos: () => {
        $('#btnTabSiguientePagos').click(() => {
            $('.nav-tabs a[href="#pagos-tab"]').tab('show')
        });
        $('#btnTabAtrasClientes').click(() => {
            $('.nav-tabs a[href="#clientes-tab"]').tab('show')
        });
    },
    showTabPagos: () => {
        $('#btnTabAtrasArticulo').click(() => {
            $('.nav-tabs a[href="#articulos-tab"]').tab('show')
        });
    },
    valTipoTarjeta: () => {
        $('#seltarjeta').change(() => {
            $('input[name=txtTipoTarjeta]').val($('#seltarjeta').val());
        });
    },
    showTabDireccionPedido: () => {
        $('#btnContinuarDatosPedidos').click(() => {
            $('.nav-tabs a[href="#nav-dir-entrega"]').tab('show');
        });
        $('#btnAtrasDatosPedido').click(() => {
            $('.nav-tabs a[href="#nav-dir-entrega"]').tab('show');
        });
    },
    showTabDireccionFacturacion: () => {
        $('#btnContinuarDatosFacturacion').click(() => {
            $('.nav-tabs a[href="#nav-dir-facturacion"]').tab('show');
        });
        $('#btnAtrasDatosGeneral').click(() => {
            $('.nav-tabs a[href="#nav-home-general"]').tab('show');
        });
    },
    isRequiredFacturar: () => {
        let isRequired = VentaUsuario.validaRequiredFactura();
        let tipodeventa = $("#tipodeventa").val();
        if (typeof isRequired == 'undefined') {
            alert("Seleccione la opcion por favor 'El Cliente requiere factura'");
            return false;
        }
        else if (tipodeventa === 'seleccionet') {
            document.getElementById("tipodeventa").focus();
            MessageError("Seleccione la opcion 'Tipo de venta'");
            return false;
        }
        else {
            $("#BuscaclienteModal").modal({ backdrop: 'static', keyboard: false });
        }
    },
    clickTabArticulos: () => {
        //$('.nav li').not('.active').addClass('disabled');
        //$('.nav li').not('.active').find('a').removeAttr("data-toggle");
        console.log();
    },
    requiredFacturaClienteMostrador: () => {
        let isTrue = 'false';
        if ($('input[name=optionRequiredFact]:checked').val() == 2) {

            let name = $('#nameV').val();

            if (name.indexOf("CLIENTEMOSTRADOR") == 0) {
                $('#optionRequiredFact').focus();
                MessageError("Usted eligio que el ''cliente requiere factura'', en este momento esta el cliente ''CLIENTEMOSTRADOR'',  favor de seleccionar un cliente y con los datos de facturación");
                isTrue = 'false';
            }
            else {
                isTrue = 'true';
            }
        }
        else {
            istrue = 'true';
        }
        return isTrue;
    },
    requiredTipoFactura: () => {
        let isTrue = 'false';
        let tipodeventa = $("#tipodeventa").val();
        if (tipodeventa === 'seleccionet') {
            document.getElementById("tipodeventa").focus();
            MessageError("Seleccione la opcion 'Tipo de venta'");
            isTrue = 'false';
        }
        else {
            isTrue = 'true';
        }
        return isTrue;
    },
    disabledTab: () => {
        VentaUsuario.enabledTab();
        $('#clientes-tab-i').addClass('disabledTab');
        $('#articulos-tab-i').addClass('disabledTab');
        $('#pagos-tab-i').addClass('disabledTab');
    },
    enabledTab: () => {
        $('#clientes-tab-i').removeClass('disabledTab');
        $('#articulos-tab-i').removeClass('disabledTab');
        $('#pagos-tab-i').removeClass('disabledTab');
    },
    validOnchangeTipodeventa: () => {
        $('#tipodeventa').change(() => {
            let tipodeventa = $("#tipodeventa").val();
            if (tipodeventa === 'seleccionet') {
                VentaUsuario.disabledTab();
            }
            else {
                VentaUsuario.enabledTab();
            }
        });
    },
    loginGarantias: () => {
        $('#btnGarantias').click(() => {
            window.location.href = 'http://170.245.190.29/DMSITE/AdminLogin/Login.aspx';
        });
    },
    loginGarantiasOficinas: () => {
        $('#btnGarantiasOficinas').click(() => {
            window.location.href = 'http://10.0.128.108/Dmsite/AdminLogin/Login.aspx';
        });
    },
    ActualizarFactClienteN: function () {
        var txtIdCliente = $("input[name=txtIdCliente]").val();
        var txtName = $("#txtName").val();
        var txtAPaterno = $("#txtAPaterno").val();
        var txtAMaterno = $("#txtAMaterno").val();
        var txtRFCN = $("#txtRFCN").val();
        var txtCorreo = $("#txtCorreo").val();
        var txFechaNacimiento = $("#txFechaNacimiento").val();
        var txtcalle = $("#txtCalle").val();
        var txtNumeroInt = $("#txtNumeroInt").val();
        var txtColonia = $("#cmbColonia option:selected").text();
        var actCod_COMUNAcolonia = $("#cmbColonia").val();
        var txtNumeroExt = $("#txtNumeroExt").val();
        var txtDelegacion = $("#txtMunicipios").val();
        var txtCP = $("#txtCP").val();
        var cmbEstados = $("#txtEstados").val();
        var txtcalleFact = $("#txtCalleFact").val();
        var txtNumeroExtFact = $("#txtNumeroExtFact").val();
        var txtNumeroIntFact = $("#txtNumeroIntFact").val();
        var txtColoniaFact = $("#cmbColoniaFact option:selected").text();
        var actCod_COMUNAcoloniaFact = $("#cmbColoniaFact").val();
        var txtDelegacionFact = $("#txtMunicipiosFact").val();
        var txtCPFact = $("#txtCPFact").val();
        var cmbEstadosFact = $("#txtEstadosFact").val();

        var txtTelCasa = $("#txtTelCasa").val();
        var txtTelOfc = $("#txtTelOfc").val();
        var txtNoCelular = $("#txtNoCelular").val();

        var cliente = {
            "Id": txtIdCliente.toString(),
            "Nombre": txtName.toString() + " " + txtAPaterno.toString() + " " + txtAMaterno.toString(),
            "RFC": txtRFCN.toString(),
            "CalleNumero": txtcalle.toString(),
            "actNOMBRE": txtName.toString(),
            "actAPEPATERNO": txtAPaterno.toString(),
            "actAPEMATERNO": txtAMaterno.toString(),
            "actNUMEXT": txtNumeroExt.toString(),
            "actNUMINT": txtNumeroInt.toString(),
            "Colonia": txtColonia.toString(),
            "CP": txtCP.toString(),
            "Correo": txtCorreo.toString(),
            "actCod_REGIONestado": cmbEstados.toString(),
            "DelMun": txtDelegacion.toString(),
            "TipoCliente": "C",
            "TelCasa": txtTelCasa.toString(),
            "TelOfc": txtTelOfc.toString(),
            "NoCelular": txtNoCelular.toString(),

            "actFCHNACIMIENTOTemp": txFechaNacimiento.toString(),
            "CalleNumeroFact": txtcalleFact.toString(),
            "actNUMEXTFact": txtNumeroExtFact.toString(),
            "actNUMINTFact": txtNumeroIntFact.toString(),
            "ColoniaFact": txtColoniaFact.toString(),
            "CPFact": txtCPFact.toString(),
            "actCod_REGIONestadoFact": cmbEstadosFact.toString(),
            "DelMunFact": txtDelegacionFact.toString(),
            "actCod_REGIONestadoFact": cmbEstadosFact.toString(),
            "actCod_COMUNAcoloniaFact": actCod_COMUNAcolonia.toString(),
            "actCod_COMUNAcolonia": actCod_COMUNAcoloniaFact.toString(),
        };
        $.ajax({
            type: "POST",
            url: '/VentasUsuario/GetUpdateFactInfoCliente',
            data: "{'clientes':" + JSON.stringify(cliente) + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                if (result === "3" || (result === "1" || result > 3)) {
                    //alert('Se realizo la actualización del cliente');
                    console.log('Se realizo la actualización del cliente');

                    // $("#BuscaclienteModal").modal('hide');
                }
                else {
                    console.log('No Se actualizo el cliente ');
                }
            },
            error: function (xhr) {
                console.log(xhr.responseText);
                alert("Error has ocurred..");
            }
        });
    },
    MostrarModalColonia: function (Tipo) {
        VentaUsuario.CaseModal(Tipo);
    },
    CaseModal: function (Tipo) {
        if (Tipo == "") {
            $("#modalCPVenta").modal('show');
        }
        if (Tipo == "Fact") {
            $("#modalCPVentaFact").modal('show');
        }
        //if (Tipo == "Add") {
        //    $("#modalCPVentaAdd").modal('show');
        //}
    },

}

$(document).ready(function () {
    var pagelocation = window.location.href;
    var string = pagelocation;
    var cargascript = string.includes("Ventas");
    if (cargascript) {
        var heightarticulos = screen.height / 3.1;
        document.getElementById('tbBodyIn').style.height = heightarticulos + "px";
    }
    $('#divRFC').append("<input type='text' class='form-control focus' id='txtRFC' maxlength='13' autofocus disabled>");
    //document.getElementById("txtRFC").focus();
    VentaUsuario.disabledCheckRfc();
    VentaUsuario.GetUsuario();
    VentaUsuario.DeleteClientesEncontrados();
    VentaUsuario.SelectedDatosIguales();
    VentaUsuario.CapturarCliente();
    VentaUsuario.SeleccionarCliente();
    VentaUsuario.ActualizarClienteN();
    VentaUsuario.CloseModalCliente();
    VentaUsuario.CloseModalClienteNew();
    VentaUsuario.CapturarClienteN();
    VentaUsuario.ClearDatosCliente();
    VentaUsuario.ClonarVenta();
    VentaUsuario.SearchVentaC();
    VentaUsuario.ClearBtnclienteAdd();
    VentaUsuario.SolonumerosCP();
    VentaUsuario.SoloNumeroLetrasRFC();
    VentaUsuario.onchangeCheckAdd();
    VentaUsuario.actionCheck();
    VentaUsuario.showModalCliente();
    VentaUsuario.onchangeTipoPersona();
    VentaUsuario.loadDataListFacturacion();
    //VentaUsuario.disabledSelectFacturacion();
    VentaUsuario.showTabVentasSiguienteClientes();
    VentaUsuario.showTabActive();
    VentaUsuario.showTabArticulos();
    VentaUsuario.showTabPagos();
    VentaUsuario.showTabDireccionPedido();
    VentaUsuario.showTabDireccionFacturacion();
    VentaUsuario.valTipoTarjeta();
    VentaUsuario.validOnchangeTipodeventa();
    Ventas.modalBuscacliente();
    VentaUsuario.loginGarantias();
    VentaUsuario.loginGarantiasOficinas();
});

//Devuelve el regex para validar una fecha.
// - dividido en variables para que se entienda y se pueda mantener/editar.
//
function regexValidarFecha() {
    let sep = "[/]",

        dia1a28 = "(0?[1-9]|1\\d|2[0-8])",
        dia29 = "(29)",
        dia29o30 = "(29|30)",
        dia31 = "(31)",

        mes1a12 = "(0?[1-9]|1[0-2])",
        mes2 = "(0?2)",
        mesNoFeb = "(0?[13-9]|1[0-2])",
        mes31dias = "(0?[13578]|1[02])",

        diames29Feb = dia29 + sep + mes2,
        diames1a28 = dia1a28 + sep + mes1a12,
        diames29o30noFeb = dia29o30 + sep + mesNoFeb,
        diames31 = dia31 + sep + mes31dias,
        diamesNo29Feb = "(?:" + diames1a28 + "|" + diames29o30noFeb + "|" + diames31 + ")",

        anno1a9999 = "(0{2,3}[1-9]|0{1,2}[1-9]\\d|0?[1-9]\\d{2}|[1-9]\\d{3})",
        annoMult4no100 = "\\d{1,2}(?:0[48]|[2468][048]|[13579][26])",
        annoMult400 = "(?:0?[48]|[13579][26]|[2468][048])00",
        annoBisiesto = "(" + annoMult4no100 + "|" + annoMult400 + ")",

        fechaNo29Feb = diamesNo29Feb + sep + anno1a9999,
        fecha29Feb = diames29Feb + sep + annoBisiesto,

        fechaFinal = "^(?:" + fechaNo29Feb + "|" + fecha29Feb + ")$";

    return new RegExp(fechaFinal);
}

//Valida una fecha ingresada como "m/d/aaaa"
// - Si no es válida, devuelve false
// - Si es válida, devuelve un objeto {d:"día",m:"mes",a:"año",date:date}
// - Parámetro: UTC (opcional) si se debe devolver {date:(date)} en UTC
//
function validarFecha(texto, UTC = false) {
    let fechaValida = regexValidarFecha(),
        // fechaValida = /^(?:(?:(0?[1-9]|1\d|2[0-8])[/](0?[1-9]|1[0-2])|(29|30)[/](0?[13-9]|1[0-2])|(31)[/](0?[13578]|1[02]))[/](0{2,3}[1-9]|0{1,2}[1-9]\d|0?[1-9]\d{2}|[1-9]\d{3})|(29)[/](0?2)[/](\d{1,2}(?:0[48]|[2468][048]|[13579][26])|(?:0?[48]|[13579][26]|[2468][048])00))$/,
        grupos;

    if (grupos = fechaValida.exec(texto)) {
        //Unir día mes y año desde los grupos que pueden haber coincidido
        let d = [grupos[1], grupos[3], grupos[5], grupos[8]].join(''),
            m = [grupos[2], grupos[4], grupos[6], grupos[9]].join(''),
            a = [grupos[7], grupos[10]].join(''),
            date = new Date(0);

        //Obtener la fecha en formato local o UTC
        if (UTC) {
            date.setUTCHours(0);
            date.setUTCFullYear(a, parseInt(m, 10) - 1, d);
        } else {
            date.setHours(0);
            date.setFullYear(a, parseInt(m, 10) - 1, d);
        }

        //Devolver como objeto con cada número por separado
        return true;
    }
    return false; //No es fecha válida
}
function validateDate(testdate) {
    var rgexp = /(^(((0[1-9]|1[0-9]|2[0-8])[-](0[1-9]|1[012]))|((29|30|31)[-](0[13578]|1[02]))|((29|30)[-](0[4,6,9]|11)))[-](19|[2-9][0-9])\d\d$)|(^29[-]02[-](19|[2-9][0-9])(00|04|08|12|16|20|24|28|32|36|40|44|48|52|56|60|64|68|72|76|80|84|88|92|96)$)/;
    return rgexp.test(testdate);
}

function LlenarDdl() {
    var listaArticulos = modeloArticulosBaseView.ListArticulos;
    $("#articulos").html("")
}
function AddCliente() {
    var x = document.getElementById("altacliente");
    var y = document.getElementById("contenidobusqueda");
    if (x.style.display === "none") {
        y.style.display = "none";
        x.style.display = "block";

    } else {

    }
}
function ClosemodCli() {
    var x = document.getElementById("contenidobusqueda");
    var y = document.getElementById("altacliente");
    if (x.style.display === "none") {
        y.style.display = "none";
        x.style.display = "block";

    } else {

    }
}
function showlblrfc() {
    lblrfc = document.getElementById("rfcalta");
    inprfc = document.getElementById("rfcaltainp");
    check = document.getElementById("checkrfc");
    if ($('input[name=optionRequiredFact]') == 2) {
        lblrfc.style.display = 'block';
        inprfc.style.display = 'block';
        $('#tipopersona').removeAttr('disabled');
    }
    else {
        lblrfc.style.display = 'none';
        inprfc.style.display = 'none';
        $('#tipopersona').prop("disabled", true);
    }
}
function FormaPagoC() {
    var opt = document.querySelector('#formapago option:checked');
    var lbtarjeta = document.getElementById("lbtarjeta");
    var seltarjeta = document.getElementById("seltarjeta");
    var lbmsi = document.getElementById("lbmsi");
    var selmsi = document.getElementById("selmsi");
    var lbtermi = document.getElementById("lbtermi");
    var selterminatarjeta = document.getElementById("selterminatarjeta");
    var lbmontoPB1 = document.getElementById("montoPB");
    var lbmontoPBtext = document.getElementById("lbmontoPB");
    var formapago = opt.value;
    switch (formapago) {
        case "5": //Terminal PROSA
        case "4": //American Express
        case "3": //Terminal Bancomer
        case "6": //Terminal Santander
        case "2": //Terminal Banamex
            lbtarjeta.style.display = "block";
            seltarjeta.style.display = "block";
            lbmsi.style.display = "block";
            selmsi.style.display = "block";
            lbmsi.style.display = "block";
            lbtermi.style.display = "block";
            selterminatarjeta.style.display = "block";
            break;
        case "1": //Efectivo
        case "7": //Deposito / Cheque
        case "8": //Transferencia Electronica
        case "10": //Compensación
            lbtarjeta.style.display = "none";
            seltarjeta.style.display = "none";
            lbmsi.style.display = "none";
            selmsi.style.display = "none";
            lbmsi.style.display = "none";
            lbtermi.style.display = "none";
            selterminatarjeta.style.display = "none";
            //seltarjeta.value = "";
            document.getElementById("seltarjeta").value = "";
            lbmontoPB1.style.display = "none";
            lbmontoPBtext.style.display = "none";
            //cleanGetPoints();
            break;
        case "9": //Monedero Payback
            if (ExistPaymentPayback()) {
                MessageError("No se puede agregar mas de un pago con Monedero Payback");
                return;
            }//Revisa si existe un pago con monedero Payback 
            else {
                $("#idSecret").show();
                $("#textsecret").show();
                lbtarjeta.style.display = "none";
                seltarjeta.style.display = "none";
                lbmsi.style.display = "none";
                selmsi.style.display = "none";
                lbmsi.style.display = "none";
                lbtermi.style.display = "none";
                selterminatarjeta.style.display = "none";
                //seltarjeta.value = "";
                document.getElementById("seltarjeta").value = "";
                lbmontoPB1.style.display = "block";
                lbmontoPBtext.style.display = "block";
                cleanGetPoints();
                $("#GetPaymentModal").modal({ backdrop: 'static', keyboard: false });
            }
            break;
        default:

    }
}
///////////Pago Comienza
function CapPago() {
    event.preventDefault();
    var table = $("#tblCPagosEnc");
    var opcionpago = $('#formapago').val();
    var montopagar = document.getElementById("montop").value;
    var montoPayback = $("#montoPB").val();
    var montototalPedido = Number($("#tventa").val().replace("$", "").replace(",", ""));
    if (montopagar <= 0.0) {
        MessageError("No puedes agregar un pago en 0");
        return;
    }
    if ((parseFloat(montopagar) > parseFloat(montototalPedido))) {
        calculasaldos();
        AlertNotification("No se puede agregar un pago mayor al total del pedido, se puede abonar maximo: " + $("#montop").val(), "", "error");
        return;
    }
    else {
        if (CheckPayments(montototalPedido, montopagar) < 0) {
            calculasaldos();
            AlertNotification("No se puede agregar un pago mayor al total del pedido, se puede abonar maximo: " + $("#montop").val(), "", "error");
            return;
        }
    }
    if (opcionpago == 1 ||
        opcionpago == 7 ||
        opcionpago == 8 ||
        opcionpago == 9 ||
        opcionpago == 10) {
        if (opcionpago == 9) {
            if (montoPayback == "") {
                MessageError("El monto a pagar no puede ser mayor al monto Payback ");
                return;
            } else if (parseInt(montopagar) > parseInt(montoPayback)) {
                MessageError("El monto a pagar no puede ser mayor al monto Payback ");
                return;
            }
        }
        var formapago = document.getElementById("formapago").value;
        var montop = document.getElementById("montop").value;
        var Cuenta = "";
        var tipotarjeta = "";
        var Msi = ""
        //document.getElementById("formapago").value = "1";
        document.getElementById("montop").value = "";
        document.getElementById("formapago").focus();
        document.getElementById("montoPB").style.display = "none";
        document.getElementById("lbmontoPB").style.display = "none";
        //cleanGetPoints();
    } else {

        var formapago = $('#formapago').val();
        var montop = document.getElementById("montop").value;
        var Cuenta = document.getElementById("selterminatarjeta").value;
        var tipotarjeta = document.getElementById("seltarjeta").value;
        var Msi = document.getElementById("selmsi").value;
        if (Msi == "") { Msi = "0" } else { }
        var tipotar = (tipotarjeta == "") ? false : true;
        var digitoscuenta = (Cuenta.length != 4) ? false : true;
        if (tipotar == false) {
            toastr.options.timeOut = 4000;
            toastr.warning('Debe seleccionar un tipo de tarjeta');
            return;
        }
        if (digitoscuenta == false) {
            toastr.options.timeOut = 4000;
            toastr.warning('Debe teclear los ultimos 4 digitos de la tarjeta/cuenta');
            return;
        }
        cleanpago(opcionpago);
    }

    if (montop == "") {
        MessageError("Por favor agrega el monto");
        return;
    }
    var i = 1
    var i = $('#tblCPagosEnc tr').length;
    //if (nofila == 1) { i =1  } else { i = nofila }
    const formatcu = { style: 'currency', currency: 'USD' };
    const numberFormat = new Intl.NumberFormat('en-US', formatcu);

    let nameFormaPago = '';
    nameFormaPago = VentaUsuario.getFormaPago(formapago);

    table.append('<tr id="rowp' + i + '"> <td>' + nameFormaPago + "" + "</td> <td>" + numberFormat.format(montop) + "</td> <td>" + Cuenta + "</td> <td>" + tipotarjeta + "</td> <td> " + Msi + '</td> <td><button type="button" name="remove" id="' + i + '" class="btn btn-danger btn-sm btn_remopago"><i class="fa fa-trash" aria-hidden="true"></i>Eliminar</button></td> </tr>');
    calculasaldos();
    var modoventa = LoadPaymentMethods();
}
function cleanpago(tipo) {
    document.getElementById("formapago").value = tipo;
    document.getElementById("montop").value = "";
    document.getElementById("selterminatarjeta").value = "";
    document.getElementById("selmsi").value = "";
    document.getElementById("seltarjeta").value = "";
    document.getElementById("seltarjeta").value = "Seleccione tipo de tarjeta...";
    document.getElementById("formapago").focus();
}
$(document).ready(function () {
    var dtToday = new Date();
    var month = dtToday.getMonth() + 1;
    var day = dtToday.getDate();
    var year = dtToday.getFullYear();
    if (month < 10)
        month = '0' + month.toString();
    if (day < 10)
        day = '0' + day.toString();
    var maxDate = year + '-' + month + '-' + day;
    $('#fechat').attr('min', maxDate);
    $("#filtrosart").hide();
    $(document).on('click', '.btn_remopago', function () {
        var button_id = $(this).attr("id");
        var DateSelected = $('#rowp' + button_id + '').find('td:first').html();
        //cuando da click obtenemos el id del boton
        $('#rowp' + button_id + '').remove(); //borra la fila
        //limpia el para que vuelva a contar las filas de la tabla
        $("#adicionados").text("");
        //REVISA SI LA FORMA DE PAGO ES PAYBACK PARA LIMPIAR CAMPOS 
        if (DateSelected == "Monedero Payback") { cleanGetPoints() }
        var nFilas = $("#tblCPagosEnc tr").length;
        $("#adicionados").append(nFilas - 1);
        $("#montop").removeClass("text-danger");
        calculasaldos();

    });
});
function calculasaldos() {
    var saldop = 0;
    var totalventa = 0;
    var pagado = 0;
    var cambio = 0;
    $('#tblCPagosEnc tr').each(function () {

        var totalacomulado = $(this).find("td").eq(1).html();
        if (totalacomulado == "" || totalacomulado == undefined) {

        } else {
            pagado += Number($(this).find("td").eq(1).html().replace("$", "").replace(",", ""));
            idp = Number($(this).find("td").eq(0).html());
        }
    });

    totalventa = Number($("#tventa").val().replace("$", "").replace(",", "")); //totaldeventa
    if (pagado > totalventa) {
        cambio = pagado - totalventa;
    }

    saldop = totalventa - pagado;                                               //pagado y saldo pendiente
    $("#sventa").val("$" + saldop);
    $("#pventa").val("$" + Number(pagado));
    $("#montop").val(saldop);
    $("#cventa").val("$" + cambio);
    if (cambio == 0) {
        $("#montop").addClass("text-success");
    }
    else {
        $("#montop").addClass("text-danger");
    }
}
///////////Pago termina
$("#ctbienda").change(function () {
    //(string ItemCode, string IdList, string descuento, string cantidad
    var idCheckJgo = document.getElementById('juego');
    var ValuecTextboxTienda = $("#ctbienda").val();
    var ValueTextboxBodega = $("#ctbodega").val();
    var ValueExistenciaTienda = $("#ctienda1").val();
    var ddlLinea = $("#ddlPrice option:selected").val();
    var ddlArticulo = $("#modelocode").val();
    var Texboxdescuento = $("#tbDescuento").val();
    if (ValuecTextboxTienda == "") {
        CleanPrice(false, false);
        MessageError("El monto no puede quedar vacio");
        return;
    } else if (ValuecTextboxTienda != "" && ValueTextboxBodega == "") { ValueTextboxBodega = 0 }
    if (parseInt(ValueTextboxBodega) < 1) {
        if (ValidaMontos(ValuecTextboxTienda, ValueExistenciaTienda)) {
            if (parseInt(ValuecTextboxTienda) <= parseInt(ValueExistenciaTienda)) {
                var obj = { ItemCode: ddlArticulo, IdList: ddlLinea, descuento: Texboxdescuento, cantidad: ValuecTextboxTienda, Origen: "Tienda", Juego: idCheckJgo.checked };
                var Json = JSON.stringify(obj);
                ChangePrice(Json);
            } else {
                MessageError("El monto no puede ser mayor a la cantidad en existencia");
                CleanPrice(false, false);
            }
        }
    } else {
        MessageError("Solo se puede seleccionar la cantidad de un origen");
        CleanPrice(true, false);
    }


});
$("#ctbodega").change(function () {
    var idCheckJgo = document.getElementById('juego');
    var ValueTextboxBodega = $("#ctbodega").val();
    var ValuecTextboxTienda = $("#ctbienda").val();
    var ValueExistenciaBodega1 = $("#cbodega1").val();
    var ddlLinea = $("#ddlPrice option:selected").val();
    var ddlArticulo = $("#modelocode").val();
    var Texboxdescuento = $("#tbDescuento").val();
    //if (ValidaMontos(ValueTextboxBodega, ValueExistenciaBodega1)) {
    if (ValueTextboxBodega == "") {
        CleanPrice(false, false);
        MessageError("El monto no puede quedar vacio");
        return;
    } else if (ValueTextboxBodega != "" && ValuecTextboxTienda == "") { ValuecTextboxTienda = 0 }
    if (parseInt(ValuecTextboxTienda) < 1) {
        if (ValidaCeros(ValueTextboxBodega)) {
            if (ValueTextboxBodega == 0) {
                CleanPrice(true, false);
                MessageError("El valor debe ser mayor a 0");
                return false;
            } else {
                var obj = { ItemCode: ddlArticulo, IdList: ddlLinea, descuento: Texboxdescuento, cantidad: ValueTextboxBodega, Origen: "Bodega", Juego: idCheckJgo.checked };
                var Json = JSON.stringify(obj);
                ChangePrice(Json);
            }
        }
    } else {
        MessageError("Solo se puede seleccionar la cantidad de un origen");
        CleanPrice(true, false);
    }
    //}
});
function ChangePrice(JsonChangePrice) {
    $.ajax({
        type: "POST",
        url: '/Ventas/GetSelectChanchePrice',
        data: "{ 'JsonChangePrice': '" + JsonChangePrice + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var idStore = '';
            if (result != null) {

                var lsLinea = JSON.parse(result);
                $("#cbIva").val(lsLinea.IVA);
                $("#tbpunitario").val(lsLinea.PrecioUnitario);
                $("#tbDescuento").val(lsLinea.Descuento);
                $("#tbTotal").val(lsLinea.Total);
                $("#tbSubtotal").val(lsLinea.Subtotal);
                $("#tbDescuentoPorc").val(lsLinea.DescuentoPrec);
            }
            else { MessageError("No Existen datos"); }
            return false;
        },
        error: function (xhr) {
            //debugger;
            console.log(xhr.responseText);
            alert("Error has occurred..");
        }
    });
}
function ValidaMontos(montoText, montoStock) {


    if (ValidaCeros(montoText)) {
        if (montoText == 0) {
            //CleanPrice(true, false);
            $("#ctienda").val("");
            $("#ctbodega").val("");
            MessageError("El valor debe ser mayor a 0");
            return false;
        } else if (montoStock == 0) {
            //CleanPrice(true, false);
            $("#ctienda").val("");
            $("#ctbodega").val("");
            MessageError("Stock insuficiente");
            return false;
        } else {
            return true;
        }
    }
}
function ValidaCeros(montoText) {
    if (montoText.length > 1) {
        var validaCeros = montoText.substr(0, 1)
        if (validaCeros == 0) {
            $("#ctienda").val("");
            $("#ctbodega").val("");
            //CleanPrice(false, false);
            MessageError("Estructura invalida");
            return false;
        } else {
            return true;
        }
    } else {
        return true;
    }
}
function CleanPrice(CleanStock, CleanModel) {
    event.preventDefault();
    $('#ddlDosPorUno').attr("hidden", "hidden");//Oculta lista de articulos de 2 x 1
    if (CleanStock == true && CleanModel == true) { OnchangeddlModelo("Linea"); }
    if (CleanStock) {
        $("#ctienda1").val(0);
        $("#cbodega1").val(0);
    }
    $("#cbIva").val("");
    $("#tbpunitario").val("");
    $("#tbTotal").val("");
    $("#ctbienda").val("");
    $('#ctbienda').removeAttr('disabled');
    $("#ctbodega").val("");
    $("#datalistOptions").val("");
    $("#cbDescriptioJuego").val("");
    $("#tbDescuento").val("");
    $("#modelocode").val("");
    $('#datalistmodelos').val("");
    var Articulo = GetJsonArtculo();
    $("#ddlArticulo")
        .empty()
        .append($("<option></option>")
            .val("0")
            .html(":.Seleccione.:"));
    $.each(Articulo, function (key, value) {
        var option = $(document.createElement('option'));
        option.html(value.name);
        option.val(value.code);
        $("#ddlArticulo")
            .append(option);
    });
    $("#ddlLinea")
        .empty()
        .append($("<option></option>")
            .val("0")
            .html(":.Seleccione.:"));
    $("#ddlMedida")
        .empty()
        .append($("<option></option>")
            .val("0")
            .html(":.Seleccione.:"));
    var promocion = GetJsonTipoPromo();
    $("#idTypePromotion")
        .empty()
    $.each(promocion, function (key, value) {
        var option = $(document.createElement('option'));
        option.html(value.Name);
        option.val(value.Code);
        $("#idTypePromotion")
            .append(option);
    });
    PriceLoad(); //recarga lista de precios al valor Default
    var idCheckJgo = document.getElementById('juego');
    var idCheckLine = document.getElementById('idCheckLine');
    idCheckJgo.checked = false;
    idCheckLine.checked = true;
}
function CheckLine() {

    var idCheckDesc = document.getElementById('idCheckDesc');
    var idCheckLine = document.getElementById('idCheckLine');
    if (idCheckLine.checked) {
        OnchangeddlModelo("Linea");
        CleanPrice(true, false);
        idCheckDesc.checked = false;
        idCheckLine.checked = true;
        $("#articuloen").val("Linea");
    } else {
        idCheckLine.checked = true;
    }

}
function CheckDesc() {
    var idCheckLine = document.getElementById('idCheckLine');
    var idCheckDesc = document.getElementById('idCheckDesc');
    if (idCheckDesc.checked) {
        OnchangeddlModelo("Descontinuados");
        CleanPrice(true, false);
        idCheckLine.checked = false;
        idCheckDesc.checked = true;
        $("#articuloen").val("Descontinuados");
    } else {
        idCheckDesc.checked = true;
    }
}
function OnchangeddlModelo(tipo) {
    $.ajax({
        type: "POST",
        url: '/Ventas/GetSelectOnchangeModelo',
        data: "{ 'tipo': '" + tipo + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result != null) {
                var lsLinea = JSON.parse(result);
                $("#datalistOptions")
                    .empty();
                addToDataList(lsLinea);
            }
            else { alert("No existe datos"); }
        },
        error: function (xhr) {
            //debugger;
            console.log(xhr.responseText);
            alert("Error has occurred..");
        }
    });
}
//Obtener datos de datalist
// create and populate datalist
function addToDataList(data) {
    data.forEach(elem => {
        option = document.createElement("option");
        option.value = elem.name;
        option.setAttribute("data-code", elem.code);
        datalistOptions.append(option);
    });
}
function Capturar() {
    event.preventDefault();
    var TypePromo = $("#idTypePromotion option:checked").val();
    var Promocion = "";
    var table = $("#tbBody");
    $('#tbDescuento').attr("hidden", true);
    $('#tbDescuento').removeAttr("readonly");
    var id = 0;
    var saldo = 0;
    var totalventa = 0;
    var descuento = 0;
    var tbtotal = Number($("#tbTotal").val().replace("$", "").replace(",", ""));
    var ddlLinea = $("#ddlPrice option:selected").text();
    var modelo = $('#datalistmodelos').val();
    var ExistTienda = false;
    var ExistBodega = false;
    if (modelo == undefined || modelo == "") {
        MessageError("Debe agregar un modelo correcto");
        return;
    }
    if (ddlLinea == undefined || ddlLinea == ":.Seleccione.:") {
        MessageError("Debe seleccionar una lista de precios válida");
        return;
    }
    else if (tbtotal > 0 || tbtotal != "") {
        $('#tbBodyIn tr').each(function () {


            var total = $(this).find("td").eq(11).html();
            if (total == "" || total == undefined) {

            } else {
                totalventa += Number($(this).find("td").eq(11).html().replace("$", "").replace(",", ""));
                descuento += Number($(this).find("td").eq(8).html().replace("$", "").replace(",", ""));
                saldo += Number($(this).find("td").eq(11).html().replace("$", "").replace(",", ""));
                id = Number($(this).find("td").eq(1).html());
            }
        });

        var itemcodevalue = $("#modelocode").val();
        var descripcionitem = $('#datalistmodelos').val();
        var idCheckJgo = document.getElementById('juego');
        var ValuecTextboxTienda = $("#ctbienda").val();
        var ValueTextboxBodega = $("#ctbodega").val();
        var ddlArticulo = $("#modelocode").val();
        var ddlModelo = $('#datalistmodelos').val();

        var Texboxdescuento = $("#tbDescuento").val();
        var origen = GetOrigen(ValuecTextboxTienda, ValueTextboxBodega);
        var idCheckJgo = document.getElementById('juego');
        if (origen == "Favor de insertar un monto válido") {
            MessageError(origen);
            return;
        } else {
            var obj = { ItemCode: ddlArticulo, IdList: ddlLinea, descuento: Texboxdescuento, cantidad: origen == "Tienda" ? ValuecTextboxTienda : ValueTextboxBodega, Origen: origen, Juego: idCheckJgo.checked };
            var Json = JSON.stringify(obj);
        }
        if (origen == "Tienda") {
            var ValueExistenciaTienda = $("#ctienda1").val();
            var ValuecTextboxTienda = $("#ctbienda").val();
            var Addtiendasinstock = ValidaMontos(ValuecTextboxTienda, ValueExistenciaTienda)
            if (Addtiendasinstock == false) {
                return;
            }
        }
        if (TypePromo == "DosPorUno") {
            var ItemCode = $("#ddlDosPorUno option:checked").val();
            var ItemName = $("#ddlDosPorUno option:checked").text();
            if (ItemName == "Seleccione artículo") {
                MessageError("Para continuar debe seleccionar un Articulo de la promociòn");
                return;
            } else {
                var Prom2X1 = { TipoPromocion: TypePromo, ItemCode: ItemCode, ItemName: ItemName };
                Promocion = JSON.stringify(Prom2X1);
            }
        }

        $.ajax({
            type: "POST",
            url: '/Ventas/GetSelectedJgo',
            data: "{ 'JsonPrice': '" + Json + "','Name': '" + ddlModelo + "','TipoPromocion': '" + Promocion + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                // Show  container
                $("#loader").show();
            },
            success: function (result) {
                var idStore = '';
                if (result != null) {

                    var lsLinea = JSON.parse(result);
                    var i = id + 1;
                    var row, cell;
                    lsLinea.forEach(function (element) {
                        if (element.CTienda > 0) { ExistTienda = true; }
                        else if (element.CBodega > 0) { ExistBodega = true; }
                        if (!idCheckJgo.checked) { element.Lista = $("#ddlPrice option:selected").text(); }

                        table.append('<tr width="1000"  id="row' + i + '"><td width="1000">' + element.Modelo + "</td><td hidden='" + "hidden" + "'>" + i + "</td><td>" + element.Juego + "</td><td>" + element.Cantidad + "</td><td>" + element.CTienda + "</td><td>" + element.CBodega + "</td><td>" + element.Lista + "</td><td>" + element.PrecioUnitario + "</td><td>" + element.Descuento + "</td><td>" + element.Subtotal + "</td><td>" + element.IVA + "</td><td>" + element.Total + "</td><td hidden='hidden'>" + element.ItemCode + '</td><td><button type="button" name="remove" id="' + i + '" class="btn btn-danger btn-sm btn_remove1"><i class="fa fa-trash" aria-hidden="true"></i>Eliminar</button></td></tr>');

                        descuento += Number(element.Descuento.replace("$", "").replace(",", ""));//suma descuento por Item
                        totalventa += Number(element.Total.replace("$", "").replace(",", "")); //Suma total de la venta
                        saldo += Number(element.Total.replace("$", "").replace(",", "")); //Suma Saldo Venta

                        var Desc = round((descuento + Number($("#tbDescuentoPorc").val().replace("$", "").replace(",", ""))), 2);
                        const formatcu = { style: 'currency', currency: 'USD' };
                        const numberFormat = new Intl.NumberFormat('en-US', formatcu);
                        numberFormat.format(totalventa);
                        $("#dventa").val(numberFormat.format(Desc));
                        $("#tventa").val(numberFormat.format(totalventa));
                        $("#sventa").val(numberFormat.format(saldo));
                        $("#montop").val(numberFormat.format(saldo));
                        i++;
                    });
                    calculasaldos();
                    CleanPrice(true, true);
                    if (ExistBodega && ExistTienda) { AlertNotification('Los articulos son de distinto Origen', '', 'warning') }
                    $('#tbBodyIn').scrollTop($('#tbBodyIn').prop('scrollHeight'));
                }
                else { alert("Existen datos"); }
                return false;
            },
            complete: function (data) {
                // Hide  container
                $("#loader").hide();
            },
            error: function (xhr) {
                //debugger;
                console.log(xhr.responseText);
                alert("Error has occurred..");
            }
        });
    }
}
function GetCantidad() {
    var cantidad = "";

    if ($("#ctbienda").val() > 0) {
        cantidad = $("#ctbienda").val();
    } else { cantidad = $("#ctbodega").val(); }
    return cantidad;

}
$(document).on('click', '.btn_remove1', function () {
    var button_id = $(this).attr("id");
    //cuando da click obtenemos el id del boton
    $('#row' + button_id + '').remove(); //borra la fila
    //limpia el para que vuelva a contar las filas de la tabla
    //$("#adicionados").text("");
    var nFilas = $("#tbBody tr").length;
    $("#adicionados").append(nFilas - 1);
    ActualizaMontos();
    $("#tblCPagosEnc > tbody").empty();//Limpia la tabla de pagos y calcula el saldo pendiente
    calculasaldos();
});
function ActualizaMontos() {
    var id = 0;
    var saldo = 0;
    var totalventa = 0;
    var descuento = 0;
    $('#tbBodyIn tr').each(function () {

        var total = $(this).find("td").eq(11).html();
        if (total == "" || total == undefined) {
        } else {
            totalventa += Number($(this).find("td").eq(11).html().replace("$", "").replace(",", ""));
            descuento += Number($(this).find("td").eq(8).html().replace("$", "").replace(",", ""));
            saldo += Number($(this).find("td").eq(11).html().replace("$", "").replace(",", ""));
            id = Number($(this).find("td").eq(1).html());
        }

    });

    $("#tventa").val(totalventa);
    $("#sventa").val(saldo);
    $("#dventa").val(descuento);

}
function Loadpreviewer() {
    event.preventDefault();
    var txtName = $('#nameV').val();
    var txtRFCN = $('#rfcV').val();
    var txtCalle = $('#direccionV').val();
    var txtid = $('#txtIdCliente').val();
    var medio = document.querySelector('#comomed option:checked');
    var fechaentrega = $('#fechat').val();
    let valTipoVenta = $('input[name=optionRequiredTipoVenta]:checked').val();
    let cantPagos = $('#tblCPagosEnc tr').length;
    let valFormaPago = $('#formapago').val();
    let montop = parseInt($('#montop').val());

    if (cantPagos > 1) {        //Si seleciona varias formas de Pagos
        let vet = [];
        let tempVet = [];
        let monto = 0, TipoPago = '', TipoTarjeta = '', temp = 0, GenericoRFC = '';
        GenericoRFC = $('#rfcV').val();

        $('#tblCPagosEnc tr').each((row, tr) => {
            if ($(tr).find('td:eq(1)').text().replace("$", "").replace(",", "") == '') {

            }
            else {
                temp = parseInt($(tr).find('td:eq(1)').text().replace("$", "").replace(",", ""));
                vet.push(temp);
            }
        });
        tempVet = vet.sort(function (a, b) { //Se ordenan los datos de menor a mayor
            return a - b
        });

        let cont = 0;
        let tip
        let esTipoEfectivo = false;
        $('#tblCPagosEnc tr').each((row, tr) => {        //Se realiza comparacion si hay dos cantidades mayores y una de ellas es Efectivo se toma como forma de Pago Efectivo,  tambien se extrae la forma de Pago mayor.
            if ($(tr).find('td:eq(1)').text().replace("$", "").replace(",", "") == '') {

            }
            else {
                temp = parseInt($(tr).find('td:eq(1)').text().replace("$", "").replace(",", ""));
                if (tempVet[tempVet.length - 1] == temp) {
                    cont++;
                    TipoPago = $(tr).find('td:eq(0)').text();

                }
                if (TipoPago.toString() == "Efectivo") {
                    TipoPago = $(tr).find('td:eq(0)').text();
                    esTipoEfectivo = true;
                }
                if (TipoPago.toString() != "Efectivo" && temp == tempVet[tempVet.length - 1] && esTipoEfectivo == false) {
                    TipoPago = $(tr).find('td:eq(0)').text();

                    if (TipoPago == "Terminal Banamex" ||         //Si son tarjetas de Credito se extare si es de Credito o de Debito.
                        TipoPago == "Terminal Santander" ||
                        TipoPago == "Terminal Bancomer" ||
                        TipoPago == "American Express" ||
                        TipoPago == "Terminal PROSA") {
                        TipoTarjeta = $(tr).find('td:eq(3)').text();

                        TipoTarjeta == "Credito" ? TipoPago = 4 : TipoPago = 28;
                    }

                }

            }
        });

        if (esTipoEfectivo == true)
            TipoPago = 'Efectivo';

        valFormaPago = VentaUsuario.getNumberFormaPago(TipoPago);

        if (montop > 0) {
            valTipoVenta = 'PPD';
            if (GenericoRFC == 'XAXX010101000') {
                valFormaPago = '99';
            }
        }
    }


    if (typeof valTipoVenta == 'undefined') {
        MessageError("Seleccione por favor la opcion 'Modo de venta'"); return

    }
    else {
        VentaUsuario.loadDataListFormaPago(valTipoVenta, valFormaPago);
    }

    if (txtName == "") { MessageError("Debe seleccionar un cliente para continuar"); return; }
    if (txtid == "72") { MessageError("Debe seleccionar un cliente Válido para continuar"); return; }
    if (medio.value == "") { MessageError("Debe seleccionar el medio por el cuál el cliente de enteró de Nosotros"); return; }
    if (fechaentrega == "") { MessageError("Debe seleccionar la fecha estimada de entrega"); return; }

    var currentdata = LoadCapturedItems();
    var currentpayments = LoadPaymentMethods();
    if (currentdata <= 1) { MessageError("No se pude crear un pedido sin articulos"); return; }
    if (currentpayments <= 1) { MessageError("No se pude crear un pedido sin pagos"); return; }
    CheckAcumulation();
    $("#idNombreC").val(txtName);
    $("#idRFCC").val(txtRFCN);
    $("#idDireccionEC").val($('#direccionV').val());
    $("#idDireccionFacC").val($('#direccionF').val());
    $("#ConfirmarventaModal").modal({ backdrop: 'static', keyboard: false });

}
function LoadCapturedItems() {
    $("#idTableBodyPreview tbody").empty();
    var countrows = $('#tbBody tr').length;
    $('#tbBodyIn tr').each(function () {

        var modelo = $(this).find("td").eq(0).html();
        var juego = $(this).find("td").eq(2).html();
        var cantidad = $(this).find("td").eq(3).html();
        var Ctienda = $(this).find("td").eq(4).html();
        var Cbodega = $(this).find("td").eq(5).html();
        var total = $(this).find("td").eq(11).html();
        if (total == "" || total == undefined) {

        } else {
            var table = $("#idTableBodyPreview");
            table.append("<tr><td>" + modelo + "</td><td>" + juego + "</td><td>" + cantidad + "</td><td>" + Ctienda + "</td><td>" + Cbodega + "</td><td>" + total + "</td></tr>");
        }
    });
    return countrows;
}
function LoadPaymentMethods() {
    var MontoTotal = 0;
    const formatcu = { style: 'currency', currency: 'USD' };
    const numberFormat = new Intl.NumberFormat('en-US', formatcu);
    $("#idPreviewMethodPayment tbody").empty();
    var countrowspay = $('#tbBodyPago tr').length;
    $('#tbBodyPago tr').each(function (row, tr) {

        var Formapago = $(tr).find('td:eq(0)').text();
        var Monto = $(tr).find('td:eq(1)').text().replace('$', '').replace(',', '');
        var cuenta = $(tr).find('td:eq(2)').text();
        var TipoTarjeta = $(tr).find('td:eq(3)').text();
        //console.log(Formapago);
        //console.log(Monto);
        if (Formapago == "" || Formapago == undefined) {

        } else {
            MontoTotal += Number(Monto);
            var table = $("#idPreviewMethodPayment");
            table.append("<tr><td>" + Formapago + "</td><td>" + numberFormat.format(Monto) + "</td><td>" + cuenta + "</td><td>" + TipoTarjeta + "</td></tr>");
        }
    });

    ValidateAmountPaid(MontoTotal);
    return countrowspay;
}
function ValidateAmountPaid(MontoT) {
    var totalventa = Number($("#tventa").val().replace("$", "").replace(",", "")); //totaldeventa
    if (MontoT >= totalventa) {
        $("#idObservaciones").val("Este pedido fue ya pagado en su totalidad");
        $("#inlineRadioVenta").prop("checked", true);
    }
    if (MontoT < totalventa) {
        $("#idObservaciones").val("Este pedido corresponde a un pedido con anticipos , en caso de no ser así favor de revisar su proceso");
        $("#inlineRadioApartado").prop("checked", true);
    }

}
function MessageError(message) {
    toastr.options.timeOut = 6000;
    toastr.warning(message);
    return;
}

$("#paymetsid").on('submit', (e) => {
    e.preventDefault();

    let isTrueFact = 'true';
    var isRequiredFactura = 'true';

    if ($('input[name=optionRequiredFact]:checked').val() == 3) { //Verifica si el cliente no require factura
        isRequiredFactura = 'false';                             //Este parametro sirve para actualizar el cliente con el RFC Generico XAXX010101000
    }

    if (isTrueFact == 'true') {
        let isTrueTipoDeVenta = 'false';
        let tipodeventa = $("#tipodeventa").val();
        if (tipodeventa === 'seleccionet') {
            document.getElementById("tipodeventa").focus();
            MessageError("Seleccione la opcion 'Tipo de venta'");
            isTrueTipoDeVenta = 'false';
        }
        else {
            isTrueTipoDeVenta = 'true';
        }

        if (isTrueTipoDeVenta == 'true') {

            var TableData = new Array();
            var TableArticulos = new Array();
            // first row will be empty - so remove
            //alert(TableData);
            var countrow = $('#tblCPagosEnc tr').length;
            $('#tblCPagosEnc tr').each(function (row, tr) {
                TableData[row] = {
                    "Id": countrow
                    , "Formapago": $(tr).find('td:eq(0)').text()
                    , "Monto": $(tr).find('td:eq(1)').text().replace('$', '').replace(',', '')
                    , "Cuenta": $(tr).find('td:eq(2)').text()
                    , "Tipotarjeta": $(tr).find('td:eq(3)').text()
                    , "MSISub": $(tr).find('td:eq(4)').text()
                    , "Afiliacion": ""
                    , "FormaPago33": $(tr).find('td:eq(0)').text()
                    , "MetodoPago33": ""
                    , "TipoComp33": "P"
                    , "UsoCFDI33": "P01"
                    , "TipoRel33": "8"
                    , "CFDI_Rel33": ""
                    , "CorreoCliente": $('#txtEmailCliente').val()
                    , "CorreoUsuario": $('input[name=txtEmailUsuario]').val()
                }
            });
            TableData.shift();// End
            var Fila = $('#tbBody tr').length;
            $('#tbBody tr').each(function (row, tr) {
                TableArticulos[row] = {
                    "Id": Fila
                    , "Modelo": $(tr).find('td:eq(0)').text()
                    , "Juego": $(tr).find('td:eq(2)').text()
                    , "Cantidad": $(tr).find('td:eq(3)').text()
                    , "CantidadTienda": $(tr).find('td:eq(4)').text()
                    , "CantidadBodega": $(tr).find('td:eq(5)').text()
                    , "Lista": $(tr).find('td:eq(6)').text()
                    , "PrecioUnitario": $(tr).find('td:eq(7)').text()
                    , "Descuento": $(tr).find('td:eq(8)').text()
                    , "subTotal": $(tr).find('td:eq(9)').text()
                    , "IVA": $(tr).find('td:eq(10)').text()
                    , "Total": $(tr).find('td:eq(11)').text()
                    , "Linea": $(tr).find('td:eq(12)').text()

                }
            });
            TableArticulos.shift();

            $('#cmbMetodoPago').prop('required', 'required');
            $('#cmbFormaPago').prop('required', 'required');
            $('#cmbUsoCFDI').prop('required', 'required');

            if ($('#cmbMetodoPago').val() != -1) {
                IsTrue = true;
            }
            else
                IsTrue = false;

            if ($('#cmbFormaPago').val() != 'Seleccione forma de pago') {
                IsTrue = true;
            }
            else
                IsTrue = false;
            if ($('#cmbUsoCFDI').val() != -1) {
                IsTrue = true;
            }
            else
                IsTrue = false;

            if ($('#cmbMetodoPago').val() == '-1' || $('#cmbFormaPago').val() == '-1') {
                IsTrue = false;
            }

            if (IsTrue == false)
                MessageError('Favor de seleccionar los campos requeridos(Metodo de Pago/Forma de Pago/Uso de CFDI)');

            if (IsTrue == true) {

                Swal.fire({
                    title: 'Confirmacion',
                    text: "Guardar pedido",
                    icon: 'success',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonText: 'No',
                    confirmButtonText: 'Sí',
                    closeOnClickOutside: false
                }).then((result) => {
                    if (result.isConfirmed) {

                        var Data;
                        var IdVenta;
                        var typeofsale = document.querySelector('#tipodeventa option:checked');
                        var comments = document.getElementById("comentarios").value;
                        var comentarios = comments.replace(/['"]+/g, '');
                        /*   var comentarios2 = comentarios.replace(/^"(.+(?="$))"$/, '$1');*/
                        var fechaentrega = $('#fechat').val();
                        var medio = document.querySelector('#comomed option:checked');
                        var idstore = $('#idstore').val();
                        var idcliente = $('#txtIdClienteIndex').val();
                        var idusuario = $('#Idusuario').val();
                        var WhsID = $('#WhsID').val();
                        let txtEmailCliente = $('#txtEmailCliente').val();
                        let txtEmailUsuario = $('input[name=txtEmailUsuario]').val();
                        let cmbMetodoPago = $('#cmbMetodoPago').val();
                        let cmbFormaPago = $('#cmbFormaPago').val().toString();
                        let cmbUsoCFDI = $('#cmbUsoCFDI').val();
                        Ventas.modalBuscacliente();

                        if (cmbFormaPago == 1 ||
                            cmbFormaPago == 2 ||
                            cmbFormaPago == 3 ||
                            cmbFormaPago == 4 ||
                            cmbFormaPago == 5 ||
                            cmbFormaPago == 6 ||
                            cmbFormaPago == 8) {
                            cmbFormaPago = '0' + cmbFormaPago
                        }

                        //llena Json final
                        var addSale = {
                            "idstore": idstore,
                            "Tipodeventa": typeofsale.value,
                            "Comentarios": comentarios,
                            "Fechaentrega": fechaentrega,
                            "Medios": medio.value,
                            "Idcliente": idcliente,
                            "Idusuario": idusuario,
                            "WhsID": WhsID,
                            "ArrayArticulos": TableArticulos,
                            "ArrayPagos": TableData,
                            "TipoOperacion": "PV",
                            "Monedero": $("#idMonedero").val(),
                            "NIP": $("#idSecret").val(),
                            "CorreoUsuario": txtEmailCliente.toString(),
                            "CorreoCliente": txtEmailUsuario.toString(),
                            "MetodoPago33": cmbMetodoPago.toString(),
                            "FormaPago33": cmbFormaPago.toString(),
                            "UsoCFDI33": cmbUsoCFDI.toString(),
                            "webToken": "",
                            "IsRequiredFactura": isRequiredFactura
                        }

                        $.ajax({
                            type: "POST",
                            url: '/Ventas/AddSale',
                            data: "{'Paymentscap': '" + JSON.stringify(addSale) + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () {
                                // Show  container
                                $("#loader").show();
                            },
                            success: function (msg) {
                                var result = JSON.parse(msg);
                                if (result.MessageErrorPayback == "" || result.MessageErrorPayback == null) {
                                    if (result.MessageErrorPBKAcumulation != null) {
                                        MessageError(result.MessageErrorPBKAcumulation);
                                    }
                                    IdVenta = parseInt(result.Idventa);

                                    window.location.href = "/VentasRPTS/Download_VentaDormimundo_PDF?IDVENTA=" + IdVenta;

                                    setTimeout(Confirmacion(IdVenta), 10000);
                                }
                                else {
                                    Swal.fire({
                                        icon: 'error',
                                        title: 'Error al procesar Venta',
                                        text: result.MessageErrorPayback,
                                        //footer: '<a href>Why do I have this issue?</a>'
                                    })
                                }
                            },
                            complete: function (data) {
                                // Hide  container
                                $("#loader").hide();
                            },
                            error: function (xhr) {
                                console.log(xhr.responseText);
                                alert("Ocurrio un error al descargar el PDF por favor verifique en el modulo de CONSULTAS si se genero el pedido para descargar el PDF, de lo contrario vuelva a generar la venta...");
                                location.reload();
                            }
                        });
                    }
                });
            }
        }
    }

    function Confirmacion(IdVenta) {

        Swal.fire({
            title: 'Descarga del pdf',
            html: "¡Espere a que se descargue el pdf!",
            icon: 'success',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Cerrar',
            type: "info",
            customClass: 'swal-wide',
            closeOnClickOutside: false
        }).then((result) => {
            $("#ConfirmarventaModal").modal('hide');
            Swal.fire(
                '¡Operacion realizada!',
                'La venta ha sido realizada',
                'success'
            )
            //var Sale = {
            //    'idVenta': parseInt(IdVenta),
            //    'TipoOperacion': 'EV'
            //};

            //$.ajax({
            //    type: "POST",
            //    url: '/Ventas/UpdateSale',
            //    data: "{'Sale':" + JSON.stringify(Sale) + "}",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (result) {

            //        if (result == 1) {
            //            $("#ConfirmarventaModal").modal('hide');
            //            Swal.fire(
            //                '¡Operacion realizada!',
            //                'La venta ha sido realizada',
            //                'success'
            //            )
            //        }
            //    },
            //    error: function (xhr) {
            //        console.log(xhr.responseText);
            //        alert("Error has ocurred..");
            //    }
            //});
            if (result.isConfirmed) {
                //local
                //location.reload();
                //productivo
                let URLactual = window.location;
                let Url = URLactual.toString().split('/');
                let rutaReports = 'http://' + Url[2] + '/Ventas';
                window.location = rutaReports.replace("''", "");
            }
        });
    };
});
function CalculaVentaArticulo() {
    var saldo = 0;
    var totalventa = 0;
    var descuento = 0;
    var Total = 0;
    $('#tbBodyIn tr').each(function () {
        var total = $(this).find("td").eq(11).html();
        if (total == "" || total == undefined) {

        } else {
            totalventa += Number($(this).find("td").eq(11).html().replace("$", "").replace(",", ""));
            descuento += Number($(this).find("td").eq(8).html().replace("$", "").replace(",", ""));
            saldo += Number($(this).find("td").eq(11).html().replace("$", "").replace(",", ""));
            id = Number($(this).find("td").eq(1).html());
        }

    });
    if (totalventa == 0) {
        $("#tventa").val(tbTotal);
        $("#sventa").val(tbTotal);
        $("#dventa").val(tbDescuentoPorc);

    } else {
        var prueba = Number($("#tbTotal").val().replace("$", "").replace(",", ""));
        var prueba1 = $("#tbTotal").val().replace("$", "");
        $("#tventa").val(totalventa + Number($("#tbTotal").val().replace("$", "").replace(",", "")));
        $("#sventa").val(saldo + Number($("#tbTotal").val().replace("$", "").replace(",", "")));
        $("#dventa").val(descuento + Number($("#tbDescuentoPorc").val().replace("$", "").replace(",", "")));
    }
}
function CalculaTotal() {


    var dato = 0;
    var dato2 = 0;

    $('#tbBody tbody tr').each(function () {
        dato += Number($(this).find('td:nth-child(12)').html().replace("$", ""));
        dato2 += Number($(this).find('td:nth-child(9)').html().replace("$", ""));
    });

    $("#tventa").val(dato);
    $("#sventa").val(dato);
    $("#dventa").val(dato2);
}
$(document).ready(function () {
    //$("#modelos").click(function (event) {
    //    $('#modelos').attr("autocomplete", true);
    //    var articuloen = $('#articuloen').val();
    //    if (articuloen == "") { articuloen = 'Linea' }
    //    $('#modelos').autocomplete({
    //        maxShowItems: 5,
    //        source: function (request, response) {
    //            $.ajax({
    //                url: "/Ventas/SearchModel",
    //                type: "POST",
    //                dataType: "json",
    //                data: { Prefix: request.term, tipo: articuloen },
    //                success: function (data) {

    //                    response($.map(data, function (item) {

    //                        return { id: item.code, label: item.name, value: item.name };
    //                    }));

    //                }
    //            })
    //        },
    //        select: function (event, ui) {
    //            $("#modelocode").val(ui.item.id);
    //            var ddlModelo = ui.item.id;
    //            $.ajax({
    //                type: "POST",
    //                url: '/Ventas/GetExistencias',
    //                data: "{ 'Parametrs': '" + ddlModelo + "'}",
    //                contentType: "application/json; charset=utf-8",
    //                dataType: "json",
    //                success: function (result) {
    //                    var idStore = '';
    //                    if (result != null) {

    //                        var lsLinea = JSON.parse(result);
    //                        var date = JSON.parse(lsLinea);
    //                        $("#ctienda1").val(date.ExistenciaTienda);
    //                        $("#cbodega1").val(date.ExistenciaBodega);
    //                    }
    //                    else { alert("Existen datos"); }
    //                    return false;
    //                },
    //                error: function (xhr) {
    //                    //debugger;
    //                    console.log(xhr.responseText);
    //                    alert("Error has occurred..");
    //                }
    //            });
    //            //return false;
    //        }
    //    });
    //});

    //DATALIST MODELOS
    $("#datalistmodelos").on('input', function () {
        //$("#ctbienda").val(0);
        //$("#ctbodega").val(0);
        $("#cbIva").val("");
        $("#tbpunitario").val("");
        $("#tbTotal").val("");
        $("#tbDescuento").val("");
        var val = $('#datalistmodelos').val();
        var articuloen = $('#articuloen').val();
        if (articuloen == "") { articuloen = 'Linea' }
        var ddlModelo = $('#datalistOptions').find('option[value="' + val + '"]').data('code');
        if (ddlModelo == undefined || ddlModelo == '') {
            //nothing
        } else {
            $('#ctbienda').removeAttr('disabled');
            $("#modelocode").val(ddlModelo);
            Load_List2X1(ddlModelo);
            $.ajax({
                type: "POST",
                url: '/Ventas/GetExistencias',
                data: "{ 'Parametrs': '" + ddlModelo + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result != null) {
                        var lsLinea = JSON.parse(result);
                        var date = JSON.parse(lsLinea);
                        $("#ctienda1").val(date.ExistenciaTienda);
                        $("#cbodega1").val(date.ExistenciaBodega);
                    }
                    else { alert("Existen datos"); }
                    return false;
                },
                error: function (xhr) {
                    //debugger;
                    console.log(xhr.responseText);
                    alert("Error has occurred..");
                }
            });
        }
    });
});
//function OnchangeModelo() {
//    event.preventDefault();
//    $("#ddlModelo").change(function () {
//        var ddlModelo = $("#ddlModelo option:selected").val();
//        $.ajax({
//            type: "POST",
//            url: '/Ventas/GetExistencias',
//            data: "{ 'Parametrs': '" + ddlModelo + "'}",
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            success: function (result) {
//                var idStore = '';
//                if (result != null) {

//                    var lsLinea = JSON.parse(result);
//                    var date = JSON.parse(lsLinea);
//                    $("#ctienda1").val(date.ExistenciaTienda);
//                    $("#cbodega1").val(date.ExistenciaBodega);
//                }
//                else { alert("Existen datos"); }
//                return false;
//            },
//            error: function (xhr) {
//                //debugger;
//                console.log(xhr.responseText);
//                alert("Error has occurred..");
//            }
//        });
//    });
//}
function OnchangeListPrice() {
    /// $("#ddlPrice").change(function () {
    //var ddlModelo = $("#ddlPrice option:selected").val();
    var idCheckJgo = document.getElementById('juego');
    var ValuecTextboxTienda = $("#ctbienda").val();
    var ValueTextboxBodega = $("#ctbodega").val();
    var ValueExistenciaBodega1 = $("#cbodega1").val();
    var ddlLinea = $("#ddlPrice option:selected").val();
    var ddlModelo = $("#modelocode").val();

    var Texboxdescuento = $("#tbDescuento").val();
    var origen = GetOrigen(ValuecTextboxTienda, ValueTextboxBodega);
    if (origen == "Favor de insertar un monto válido") {
        MessageError(origen);
        return;
    } else {
        var obj = { ItemCode: ddlModelo, IdList: ddlLinea, descuento: Texboxdescuento, cantidad: origen == "Tienda" ? ValuecTextboxTienda : ValueTextboxBodega, Origen: origen, Juego: idCheckJgo.checked };
        var Json = JSON.stringify(obj);
        ChangePrice(Json);
    }
}
function CheckJuego() {
    var idCheckJgo = document.getElementById('juego');
    var ddlModelo = $("#modelocode").val();
    var TypeArt = $("#tipodeventa option:checked").text();//obtiene tipo de venta
    if (idCheckJgo.checked) {
        $.ajax({
            type: "POST",
            url: '/Ventas/GetSelectJuego',
            data: "{ 'modelo': '" + ddlModelo + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var idStore = '';
                if (result != null) {

                    var lsLinea = JSON.parse(result);
                    var lsPrice = lsLinea.ListPrice;
                    $("#cbDescriptioJuego").val(lsLinea.itemName);
                    if (lsPrice != null) {
                        if (lsLinea.MessageError == "" || lsLinea.MessageError == null) {//Nothing 
                        }
                        else {
                            AlertNotification(lsLinea.MessageError, "", "warning");
                            CleanPrice(false, false);
                            return false;
                        }
                        $("#ddlPrice")
                            .empty()
                            .append($("<option></option>")
                                .val("0")
                                .html(":.Seleccione.:"));
                        $.each(lsPrice, function (key, value) {
                            if (TypeArt == "Dormicredit" && (value.ListName.includes("Dormicre") || value.ListName.includes("Temporal Carga SMU"))) {
                                var option = $(document.createElement('option'));
                                option.html(value.ListName);
                                option.val(value.ListID);
                                $("#ddlPrice")
                                    .append(option);
                            } else if (TypeArt == "Normal" && !value.ListName.includes("Dormicre")) {
                                var option = $(document.createElement('option'));
                                option.html(value.ListName);
                                option.val(value.ListID);
                                $("#ddlPrice")
                                    .append(option);
                            }
                            //var option = $(document.createElement('option'));
                            //option.html(value.ListName);
                            //option.val(value.ListID);
                            //$("#ddlPrice")
                            //    .append(option);
                        });
                    }
                }
                else { alert("Existen datos"); }
                return false;
            },
            error: function (xhr) {
                //debugger;
                console.log(xhr.responseText);
                alert("Error has occurred..");
            }
        });
    } else {
        PriceLoad();
        $("#cbDescriptioJuego").val("");
    }
}
function GetOrigen(montoT, montoB) {
    if ((montoT == 0 && montoB == 0) || (montoT != 0 && montoB != 0)) {
        return "Favor de insertar un monto válido";
    } else if (montoT > 0) {
        return "Tienda";
    } else if (montoB > 0) {
        return "Bodega";
    }
}
function PriceLoad() {
    var TypeArt = $("#tipodeventa option:checked").text();
    var Price = GetJsonPrice();
    $("#ddlPrice")
        .empty();
    $.each(Price, function (key, value) {
        if (TypeArt == "Dormicredit" && (value.ListName.includes("Dormicre") || value.ListName.includes("Temporal Carga SMU"))) {
            var option = $(document.createElement('option'));
            option.html(value.ListName);
            option.val(value.ListID);
            $("#ddlPrice")
                .append(option);
        } else if (TypeArt == "Normal" && !value.ListName.includes("Dormicre")) {
            var option = $(document.createElement('option'));
            option.html(value.ListName);
            option.val(value.ListID);
            $("#ddlPrice")
                .append(option);
        }
    });
}
function round(num, decimales = 2) {
    var signo = (num >= 0 ? 1 : -1);
    num = num * signo;
    if (decimales === 0) //con 0 decimales
        return signo * Math.round(num);
    // round(x * 10 ^ decimales)
    num = num.toString().split('e');
    num = Math.round(+(num[0] + 'e' + (num[1] ? (+num[1] + decimales) : decimales)));
    // x * 10 ^ (-decimales)
    num = num.toString().split('e');
    return signo * (num[0] + 'e' + (num[1] ? (+num[1] - decimales) : -decimales));
}
function CheckAcumulation() {

    var monedero = $("#idMonedero").val();
    var Secret = $("#idSecret").val();
    if (monedero == "" && Secret == "") {
        Swal.fire({
            icon: 'info',
            title: '¿Cuenta con Monedero Payback para poder acumular?',
            showCancelButton: true,
            confirmButtonText: "Sí",
            cancelButtonText: "No",
            closeOnEsc: false,
            allowOutsideClick: false,
            allowEscapeKey: false,
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        })
            .then(resultado => {
                if (resultado.value) {
                    $("#GetPaymentModal").modal({ backdrop: 'static', keyboard: false });
                    $("#idSecret").hide();
                    $("#textsecret").hide();
                    // Hicieron click en "Sí"textsecret
                } else {
                    // Dijeron que no
                }
            });
    }

}
function ExistPaymentPayback() {
    var Existe = false;
    $('#tblCPagosEnc tr').each(function (row, tr) {
        var Formapago = $(tr).find('td:eq(0)').text()
        if (Formapago == "Monedero Payback") {
            Existe = true;
        }
    });
    return Existe;
}
function CancelaPedido(idVenta) {

    $.ajax({
        type: "POST",
        url: '/Ventas/CancelaPedidoInt',
        data: "{ 'idVenta': '" + idVenta + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var idStore = '';
            if (result != null) {
                window.location.href = "/VentasRPTS/Download_Cancelacion_PDF?IdDevolucion=" + result.idVenta;
                //window.location.href = "/Ventas";
                //var lsLinea = JSON.parse(result);

            }
            else { alert("Error al procesar cancelacion"); }
            return false;
        },
        error: function (xhr) {
            //debugger;
            console.log(xhr.responseText);
            alert("Error has occurred..");
        }
    });
}

function filterswatch() {
    var checkfiltro = document.getElementById("inlineCheckFilter");
    var x = document.getElementById("filtrosart");
    var activofiltro = checkfiltro.checked;
    if (activofiltro) {
        $('#filtrosart').show();
        document.getElementById("ddlArticulo").focus();
    }
    else {
        $("#filtrosart").hide();
    }

}
function OnchangeListPriceLoad() {
    PriceLoad();
}
function AlertNotification(title, text, icon) {
    Swal.fire(title, text, icon)
}



$(document).ready(function () {
    var screen = $('#loading-screen');
    configureLoadingScreen(screen);

    $('.do-request').on('click', function () {
        $.get('http://jsonplaceholder.typicode.com/posts')
            .done(function (result) {
                $('#results').text(JSON.stringify(result, null, 2));
            })
            .fail(function (error) {
                console.error(error);
            })
    })
});

function configureLoadingScreen(screen) {
    $(document)
        .ajaxStart(function () {
            screen.fadeIn();
        })
        .ajaxStop(function () {
            screen.fadeOut();
        });
}
function SolicitarDescuento() {
    var ItemCode = $("#modelocode").val();
    var idcliente = $('#txtIdClienteIndex').val();
    var idusuario = $('#Idusuario').val();
    if (ItemCode == "") {
        Swal.fire('', 'Debe seleccionar un modelo', 'alert')
        return;
    }
    if (idcliente == "") {
        Swal.fire('', 'Debe seleccionar un cliente', 'alert')
        return;
    }

    Swal.fire({
        title: '¿Desea solicitar un codigo para descuento?',
        text: 'Cantidad solicitada:',
        html:
            '<div class="row"<label for="recipient-name" class="col-form-label">Porcentaje solicitado:</label>' +
            '<input id="swal-input1" class="form-control-sm text-center mb-3" type="number" pattern="[0-9]" min="0" max="100" maxlength="5" placeholder = "0%">' + "</div></br>" +
            '<div class="row"><label for="recipient-name" class="col-form-label">Observaciones:</label>' +
            '<textarea id="tbObservaciones" class="form-control-sm mb-3" rows="1" maxlength="500" placeholder = "Observaciones"></textarea></div>'
        ,
        showDenyButton: true,
        showCancelButton: true,
        cancelButtonText: 'Cancelar',
        confirmButtonText: 'Solicitar',
        denyButtonText: '¡Ya tengo un codigo!',
        closeOnEsc: false,
        allowOutsideClick: false,
        allowEscapeKey: false,
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            var monto = document.getElementById('swal-input1').value;
            if (monto == "") {
                Swal.fire("", "El monto no puede estar vacio", "alert");
            }
            var Observaciones = document.getElementById('tbObservaciones').value;
            var IdList = $("#ddlPrice option:selected").val(); 
            var PriceSMU = document.getElementById('tbTotal').value;
            //var ItemCode = $("#modelocode").val();
            // $('#MdlInfoDesc').modal('show');
            // Swal.fire('solicitando!', '', 'success')
            $.ajax({
                type: "POST",
                url: '/Ventas/RequestApDesc',
                data: "{ 'ItemCode': '" + ItemCode + "','monto': '" + monto + "','idcliente': '" + idcliente + "','Observaciones': '" + Observaciones + "','Lista': " + IdList + ",'PriceFDO': '" + PriceSMU + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result != null) {
                        var lsLinea = JSON.parse(result);
                        if (lsLinea.Success) {
                            Swal.fire('¡Enviado!', 'Su peticion fue enviada con éxito', 'Succes')
                        }

                    }
                    else { alert("Existen datos"); }
                    return false;
                },
                error: function (xhr) {
                    //debugger;
                    console.log(xhr.responseText);
                    alert("Error al solicitar descuento");
                }
            });
        } else if (result.isDenied) {
            InsertCodigo(ItemCode, idcliente);
            //Swal.fire('Ingrese codigo', '', 'info')
        }
    })

}
function InsertCodigo(ItemCode, IdCliente) {
    // $('#myModalExito').modal('show');

    Swal.fire({
        title: 'Ingrese el codigo',
        html: '<input id="swal-input2"  class="form-control-sm text-center mb-3" type="text" maxlength="6" placeholder = "0">',
        //showDenyButton: true,
        showCancelButton: true,
        cancelButtonText: 'Cancelar',
        confirmButtonText: 'Aceptar',
        denyButtonText: '¡Ya tengo un codigo!',
        closeOnEsc: false,
        allowOutsideClick: false,
        allowEscapeKey: false,
    }).then((result) => {
        var Code = document.getElementById('swal-input2').value;
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            $.ajax({
                type: "POST",
                url: '/Ventas/InsertCodigo',
                data: "{ 'codigo': '" + Code + "','ItemCode': '" + ItemCode + "','IdCliente': '" + IdCliente + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result != null) {
                        var lsLinea = JSON.parse(result);
                        if (lsLinea.Success) {
                            $('#tbDescuento').removeAttr('hidden');
                            $('#tbDescuento').val(lsLinea.Message);
                            //$('#tbDescuento').val();
                            $('#tbDescuento').attr("readonly", "readonly");
                        } else {
                            Swal.fire("", lsLinea.Message, "alert");
                        }

                    }
                    else { alert("Existen datos"); }
                    return false;
                },
                error: function (xhr) {
                    //debugger;
                    console.log(xhr.responseText);
                    alert("Error al solicitar descuento");
                }
            });
            // $('#MdlInfoDesc').modal('show');
            // Swal.fire('solicitando!', '', 'success')
        } else if (result.isDenied) {
            Swal.fire('Ingrese codigo', '', 'info')
        }
    })
}
function Aceptar_Onclick(iddescuento, Status) {
    Swal.fire({
        title: '¿Desea continuar para autorizar el descuento solicitado?',
        showDenyButton: true,
        showCancelButton: true,
        cancelButtonText: 'No',
        confirmButtonText: 'Si',
        denyButtonText: 'Editar cantidad solicitada',
        closeOnEsc: false,
        allowOutsideClick: false,
        allowEscapeKey: false,
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            Btn_Autorizado(iddescuento, Status, "");
        } else if (result.isDenied) {
            Swal.fire({
                title: 'Ingrese nuevo descuento',
                html: '<input id="newDes"  class="form-control-sm text-center mb-3" type="text" maxlength="6" placeholder = "0">',
                showCancelButton: true,
                cancelButtonText: 'Cancelar',
                confirmButtonText: 'Aceptar',
                closeOnEsc: false,
                allowOutsideClick: false,
                allowEscapeKey: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    var newDesc = $('#newDes').val();
                    Btn_Autorizado(iddescuento, Status, newDesc);
                }
            })
        }
    })
}
function Btn_Autorizado(iddescuento, Status, Descuento) {
    var mensaje = "El descuento se acepto correctamente";
    if (Status == "2" || Status == 2) { mensaje = "El descuento se rechazo correctamente"; };
    $.ajax({
        type: "POST",
        url: '/TiendaGeneral/Btn_Aceptar',
        data: "{ 'iddescuento': '" + iddescuento + "','Status': " + Status + ",'Descuento': '" + Descuento + "' }",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result != null) {
                var lsLinea = JSON.parse(result);
                if (lsLinea.Success) {
                    toastr.options.timeOut = 60000;
                    toastr.options = {
                        positionClass: 'toast-top-center'
                    };
                    toastr.success(mensaje);
                    //window.setTimeout(function () {
                    //}, 60000);
                    window.location.href = "/TiendaGeneral/AprobDesc";
                } else {
                    MessageError("alert", lsLinea.Message, "");
                }

            }
            else { alert("Existen datos"); }
            return false;
        },
        error: function (xhr) {
            //debugger;
            console.log(xhr.responseText);
            alert("Error al solicitar descuento");
        }
    });
}
function Btn_EditAutorizacion() {

}
function Btn_SelectedTypePromotion() {
    var TypePromo = $("#idTypePromotion option:checked").val();
    if (TypePromo == "Ninguno") {
        $('#idDescJGO').removeAttr('hidden');
        $('#HiddenJgo').removeAttr('hidden');

        //$('#HiddenJgo').attr("hidden", "hidden");
        //$('#idDescJGO').attr("hidden", "hidden");
        $('#ddlDosPorUno').attr("hidden", "hidden");
    } else if (TypePromo == "Juego") {
        $('#HiddenJgo').removeAttr('hidden');
        $('#idDescJGO').removeAttr('hidden');
        $('#ddlDosPorUno').attr("hidden", "hidden");
    } else if (TypePromo == "DosPorUno") {
        $('#HiddenJgo').attr("hidden", "hidden");
        $('#idDescJGO').attr("hidden", "hidden");
        $('#ddlDosPorUno').removeAttr('hidden');
    }
}
function Load_List2X1(ItemCodeModel) {
    var existe = false;
    var Tipopromocion = GetJsonTipoPromo();
    var Promocion = GetJsonPromo();
    $.each(Promocion, function (key, value) {
        if (ItemCodeModel == value.ItemCode) {
            existe = true;
        }
    });//END EACH

    $("#idTypePromotion")
        .empty()
    $.each(Tipopromocion, function (key, value) {

        if (existe) {
            if (value.Code != "Ninguno") {
                var option = $(document.createElement('option'));
                option.html(value.Name);
                option.val(value.Code);
                $("#idTypePromotion")
                    .append(option);
                $("#ctbienda").val(0);
                $("#ctbienda").attr("disabled", "disabled"); 
            }           
            Btn_SelectedTypePromotion();
        } else {
            if (value.Code != "DosPorUno") {
                var option = $(document.createElement('option'));
                option.html(value.Name);
                option.val(value.Code);
                $("#idTypePromotion")
                    .append(option);
            }
            Btn_SelectedTypePromotion();
        }

    });//END EACH     
}

function CheckPayments(n1, n2) {
    var pagado = 0;
    $('#tblCPagosEnc tr').each(function () {
        var totalacomulado = $(this).find("td").eq(1).html();
        if (totalacomulado == "" || totalacomulado == undefined) {
        } else {
            pagado += Number($(this).find("td").eq(1).html().replace("$", "").replace(",", ""));
            idp = Number($(this).find("td").eq(0).html());
        }
    });
    var restapago = (parseFloat(n1) - parseFloat(n2));
    return (parseFloat(restapago) - parseFloat(pagado));
}
function configureLoadingScreen(screen) {
    $(document)
        .ajaxStart(function () {
            screen.fadeIn();
        })
        .ajaxStop(function () {
            screen.fadeOut();
        });
}
