 var tableT;

var Reports = {
    getVendedores: () => {
        let valTienda = $('#cmbTiendaConsult').val();        
        if (valTienda == "Todos") {
            $.ajax({
                type: "POST",
                url: "/Reportes/getVendedores",
                //data: "{'vendedor':" + JSON.stringify(Tienda) + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: (result) => {
                    console.log("result");
                    console.log(result);

                    let option = "";
                    $('#cmbVendedorConsult').empty();
                    $('#cmbVendedorConsult').append("<option value='-1'>Todos</option>");
                    for (let item in result) {
                        $('#cmbVendedorConsult').append("<option value='" + result[item].AdminUserID + "'>" + result[item].Nombre + "</option>");
                    }
                },
                error: (xhr) => {
                    console.log(xhr.responseText);
                    alert("Ha Ocurrido un Error al obtener el Vendedor");
                }
            });
        } else {
            try {
                let Tienda = {
                    'AdminStoreID': valTienda
                }

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetVendedorByStore",
                    data: "{'vendedor':" + JSON.stringify(Tienda) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        let option = "";
                        $('#cmbVendedorConsult').empty();
                        $('#cmbVendedorConsult').append("<option value='-1'>Todos</option>");
                        for (let item in result) {
                            $('#cmbVendedorConsult').append("<option value='" + result[item].AdminUserID + "'>" + result[item].Nombre + "</option>");
                        }
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        alert("Ha Ocurrido un Error al obtener el Vendedor");
                    }
                });
            } catch (ex) {
                alert(ex);
            }

        }
      
    },
    getReportsVentas: () => {

        //$('#verVendedoresBtn').click(() => {
            
        //});


        $('#btnReportsFiltro').click(() => {
            let valTienda = $('#cmbTiendaConsult').val().toString();
            let valVendedor = $('#cmbVendedorConsult').val().toString();
            let valEstatus = $('#cmbEstatusConsult').val().toString();
            let dtFechaInicioReportsV = $('#txFechaInicioReportsV').val().toString();
            let dtFechaFinReportsV = $('#txFechaFinReportsV').val().toString();

            if (dtFechaInicioReportsV == '' && dtFechaFinReportsV == '') {
                toastr.warning('Seleccione una fecha de inicio y fecha final', 'Seleccione Fechas');
                return;
            } else {
                let Reports = {
                    "AdminUserID": -1,
                    "FRCARDCODE": -1,
                    "Usuarios": valVendedor,
                    "Tiendas": valTienda,
                    "Estatus": valEstatus,
                    "Fecha1": dtFechaInicioReportsV,
                    "Fecha2": dtFechaFinReportsV
                }

                try {

                    $("#modalLoading").modal("show");

                    $.ajax({
                        type: "POST",
                        url: "/Reportes/GetReportsVentas",
                        data: "{'reports' : " + JSON.stringify(Reports) + "}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: (result) => {
                            if (result === null || result === '[]') {
                                $("#modalLoading").modal("hide");
                                AlertNotification("Ningún dato disponible en esta tabla ", "", "warning");
                                return;
                            }
                            var dataset = JSON.parse(result)
                            var columns = "";
                            if (dataset != null) {
                                var columnDefs = [];
                                //loop for populating column defs

                                //$('#tblConsultaVTxTiendaReport > thead').append('<tr>');
                                for (let k in dataset[0]) {
                                    if (columnDefs.indexOf(k) === -1) {
                                        columnDefs.push({ title: dataset[0][k] });
                                    }

                                    columns = columns + '<th>' + k.toString() + '</th>'; //
                                }

                                $('#tblConsultaVentaReport > thead').append('<tr>' + columns + '</tr>');  //Se Agrega encabezados a la tabla creada
                                //loop for populating data
                                var data = [];
                                for (let j of dataset) {
                                    var arr = [];

                                    for (let k in j) {
                                        if (columnDefs.indexOf(k) === -1) {
                                            if (k.toString() == 'ID') {
                                                arr.push(j[k]);
                                            }
                                            else {
                                                if (k.toString() == 'Fecha_Completado' ||
                                                    k.toString() == 'Fecha de Entrega' ||
                                                    k.toString() == 'Fecha' ||
                                                    k.toString() == 'FechaCFDI') {
                                                    if (j[k] == '0') {
                                                        arr.push('');
                                                    }
                                                    else {
                                                        if (j[k] === null) {
                                                            arr.push('');
                                                        }
                                                        else {
                                                            if (j[k].toString() == '1900/01/01') {
                                                                arr.push('');
                                                            }
                                                            else {
                                                                //let fecha = moment(j[k]).format("DD/MM/YYYY")

                                                                let fecha = j[k].substring(0, 10)
                                                                arr.push(fecha);
                                                            }
                                                        }
                                                        if (j[k] === '1900-01-01' ||
                                                            typeof j[k] === 'undefined' ||
                                                            j[k] == '1900-01-01' ||
                                                            j[k] === '') {
                                                            arr.push('');
                                                        }

                                                    }
                                                }
                                                else {
                                                    if (k.toString() == 'Cantidad') {
                                                        arr.push(j[k]);
                                                    }
                                                    else {
                                                        if (j[k] != null) {
                                                            if (k.toString() == 'Comentarios') {
                                                                arr.push(j[k]);
                                                            } else {
                                                                arr.push(j[k].toLocaleString('en-US', { style: 'currency', currency: 'USD' }));
                                                            }

                                                        }
                                                        else
                                                            arr.push(0);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    data.push(arr);
                                }

                                $('.DTFC_LeftWrapper').empty() //Se elimina contenido de tabla clon de datatable

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

                                var tableT = $("#tblConsultaVentaReport").dataTable({
                                    destroy: true,
                                    language: spanish,
                                    //searching: false,
                                    scrollX: true,
                                    //scrollCollapse: true,
                                    fixedColumns: true,
                                    select: false,
                                    data: data,
                                    "pageLength": 20,
                                    "lengthChange": false,
                                    //"responsive": true,
                                    "rowReorder": {
                                        "selector": 'td:nth-child(2)'
                                    },
                                    "fixedHeader": {
                                        "header": true
                                    },
                                    dom: 'Bfrtip',
                                    buttons: [
                                        'excel'
                                    ]
                                });
                            }
                            else {
                                $(".DTFC_Cloned tbody").empty();
                                $("#tblConsultaVentaReport").dataTable().empty();
                            }
                            //Tiempo para cerrar el modal loading
                            setTimeout(function () { $("#modalLoading").modal("hide"); },
                                5000);
                            //Se agrega icono al boton excel
                            $('.buttons-excel').prepend('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                        },
                        error: (xhr) => {
                            console.log(xhr.responseText);
                            alert("Ha ocurrido un error al generar la consulta del reporte de ventas");
                            $("#modalLoading").modal("hide");
                        }
                    });

                } catch (ex) {
                    alert(ex);
                    $("#modalLoading").modal("hide");
                }

            }

           
        });
    },
    getReportsFacturacion: () => {
        $('#btnReportsFactFiltro').click(() => {
            let valTienda = $('#cmbTiendaConsult').val().toString();
            let valVendedor = $('#cmbVendedorConsult').val().toString();
            let dtFechaInicioReportsV = $('#txFechaIReportsFact').val().toString();
            let dtFechaFinReportsV = $('#txFechaFReportsFact').val().toString();

            let Reports = {
                "AdminUserID": -1,
                "FRCARDCODE": -1,
                "Usuarios": valVendedor,
                "Tiendas": valTienda,
                "Fecha1": dtFechaInicioReportsV,
                "Fecha2": dtFechaFinReportsV
            }

            try {

                $("#modalLoading").modal("show");

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetReportsFacturacion",
                    data: "{'reports' : " + JSON.stringify(Reports) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
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

                            table = $("#tblConsultaFacturacionReport").dataTable({
                                destroy: true,
                                language: spanish,
                                //searching: false,
                                scrollX: true,
                                //scrollCollapse: true,
                                fixedColumns: true,
                                select: false,
                                data: data,
                                columns: [
                                    { title: "ID" },
                                    { title: "Fecha Pedido" },
                                    { title: "Fecha Factura" },
                                    { title: "Fecha Completado" },
                                    { title: "Tienda" },
                                    { title: "Venta" },
                                    { title: "Vendedor" },
                                    { title: "Cliente" },
                                    { title: "StatusVenta" },
                                    { title: "Facturado" },
                                    { title: "Total Venta" },
                                    { title: "Monto pagado" },
                                    { title: "Saldo" },
                                    { title: "Comentarios" },
                                    { title: "Estatus Factura" },
                      
                                ],
                                "pageLength": 20,
                                "lengthChange": false,
                                "responsive": true,
                                "rowReorder": {
                                    "selector": 'td:nth-child(2)'
                                },
                                "fixedHeader": {
                                    "header": true
                                },
                                dom: 'Bfrtip',
                                buttons: [
                                    'excel'
                                ]
                            });
                        }
                        else {
                            $(".DTFC_Cloned tbody").empty();
                            $("#tblConsultaFacturacionReport").dataTable().empty();
                        }
                        //Tiempo para cerrar el modal loading
                        setTimeout(function () { $("#modalLoading").modal("hide"); },
                            5000);
                        //Se agrega icono al boton excel
                        $('.buttons-excel').prepend('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        alert("Ha ocurrido un error al generar la consulta del reporte");
                        $("#modalLoading").modal("hide");
                    }
                });
            } catch (ex) {
                alert(ex);
                $("#modalLoading").modal("hide");
            }
        });
    },
    getReportsClienteAvisa: () => {
        $('#btnReportsClienteAvisaFiltro').click(() => {
            let valTienda = $('#cmbTiendaConsult').val().toString();
            let valVendedor = $('#cmbVendedorConsult').val().toString();
            let valEstatus = $('#cmbEstatusConsultAvisa').val().toString();
            let dtFechaInicioReportsV = $('#txFechaInicioReportsAvisa').val().toString();
            let dtFechaFinReportsV = $('#txFechaFinReportsAvisa').val().toString();

            let Reports = {
                "AdminUserID": -1,
                "FRCARDCODE": -1,
                "Usuarios": valVendedor,
                "Tiendas": valTienda,
                "Estatus": valEstatus,
                "Fecha1": dtFechaInicioReportsV,
                "Fecha2": dtFechaFinReportsV
            }

            try {

                $("#modalLoading").modal("show");

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetReportsClienteAvisa",
                    data: "{'reports' : " + JSON.stringify(Reports) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
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

                            table = $("#tblConsultaClienteAvisaReport").dataTable({
                                destroy: true,
                                language: spanish,
                                //searching: false,
                                scrollX: true,
                                //scrollCollapse: true,
                                fixedColumns: true,
                                select: false,
                                data: data,
                                columns: [
                                    { title: "ID" },
                                    { title: "Fecha" },
                                    { title: "Tienda" },
                                    { title: "Venta" },
                                    { title: "Vendedor" },
                                    { title: "Cliente" },
                                    { title: "Estatus Venta" },
                                    { title: "Código Artículo" },
                                    { title: "Artículo" },
                                    { title: "Lista de Precios" },
                                    { title: "Cantidad" },
                                    { title: "Precio Unitario" },
                                    { title: "Descuento" },
                                    { title: "Total Linea" },
                                    { title: "Total Venta" },
                                    { title: "Monto Pagado" },
                                    { title: "Saldo" },
                                ],
                                "pageLength": 5,
                                "lengthChange": false,
                                "responsive": true,
                                "rowReorder": {
                                    "selector": 'td:nth-child(2)'
                                },
                                "fixedHeader": {
                                    "header": true
                                },
                                dom: 'Bfrtip',
                                buttons: [
                                    'excel'
                                ]
                            });
                        }
                        else {
                            $(".DTFC_Cloned tbody").empty();
                            $("#tblConsultaClienteAvisaReport").dataTable().empty();
                        }
                        //Tiempo para cerrar el modal loading
                        setTimeout(function () { $("#modalLoading").modal("hide"); },
                            5000);
                        //Se agrega icono al boton excel
                        $('.buttons-excel').prepend('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        alert("Ha ocurrido un error al generar la consulta del reporte");
                        $("#modalLoading").modal("hide");
                    }
                });
            } catch (ex) {
                alert(ex);
                $("#modalLoading").modal("hide");
            }
        });
    },
    getReportsTotalVenta: () => {
        $('#btnReportsTVentaFiltro').click(() => {
            let valTienda = $('#cmbTiendaConsult').val().toString();
            let valVendedor = $('#cmbVendedorConsult').val().toString();
            let valEstatus = $('#cmbEstatusConsultTVenta').val().toString();
            let dtFechaInicioReportsV = $('#txFechaInicioReportsTVenta').val().toString();
            let dtFechaFinReportsV = $('#txFechaFinReportsTVenta').val().toString();

            let Reports = {
                "AdminUserID": -1,
                "FRCARDCODE": -1,
                "Usuarios": valVendedor,
                "Tiendas": valTienda,
                "Estatus": valEstatus,
                "Fecha1": dtFechaInicioReportsV,
                "Fecha2": dtFechaFinReportsV
            }

            try {

                $("#modalLoading").modal("show");

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetReportsTotalVenta",
                    data: "{'reports' : " + JSON.stringify(Reports) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
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

                            table = $("#tblConsultaTotalVentaReport").dataTable({
                                destroy: true,
                                language: spanish,
                                //searching: false,
                                scrollX: true,
                                //scrollCollapse: true,
                                fixedColumns: true,
                                select: false,
                                data: data,
                                columns: [
                                    { title: "ID" },
                                    { title: "Fecha" },
                                    { title: "Fecha CFDI" },
                                    { title: "Tienda" },
                                    { title: "Venta" },
                                    { title: "Vendedor" },
                                    { title: "Cliente" },
                                    { title: "Estatus Venta" },
                                    { title: "Total Venta" },
                                    { title: "Monto pagado" },
                                    { title: "Saldo" },
                                    { title: "Estatus Factura" }
                                ],
                                "pageLength": 5,
                                "lengthChange": false,
                                "responsive": true,
                                "rowReorder": {
                                    "selector": 'td:nth-child(2)'
                                },
                                "fixedHeader": {
                                    "header": true
                                },
                                dom: 'Bfrtip',
                                buttons: [
                                    'excel'
                                ]
                            });
                        }
                        else {
                            $(".DTFC_Cloned tbody").empty();
                            $("#tblConsultaTotalVentaReport").dataTable().empty();
                        }
                        //Tiempo para cerrar el modal loading
                        setTimeout(function () { $("#modalLoading").modal("hide"); },
                            5000);
                        //Se agrega icono al boton excel
                        $('.buttons-excel').prepend('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        alert("Ha ocurrido un error al generar la consulta del reporte");
                        $("#modalLoading").modal("hide");
                    }
                });
            } catch (ex) {
                alert(ex);
                $("#modalLoading").modal("hide");
            }
        });
    },
    getReportsVentaxArticulo: () => {
        $('#btnReportsTiendaArticuloFiltro').click(() => {
            let valTienda = $('#cmbTiendaConsult').val().toString();
            let valVendedor = $('#cmbVendedorConsult').val().toString();
            let valEstatus = $('#cmbEstatusConsultVAr').val().toString();
            let dtFechaInicioReportsV = $('#txFechaIVentaArt').val().toString();
            let dtFechaFinReportsV = $('#txFechaFVentaArt').val().toString();
            let valLineaVentaArt = $('#cmbLineaVentaArt').val().toString();
            let valMedidaVentaArt = $('#cmbMedidaVentaArt').val().toString();
            let valArticuloVentaArt = $('#cmbArticuloVentaArt').val().toString();
            let valModeloVentaArt = $('#cmbModeloVentaArt').val().toString();

            if (dtFechaInicioReportsV == '' && dtFechaFinReportsV =='') {
                toastr.warning('Debe seleccionar fecha de inicio y fecha fin para el reporte.', 'Fechas Invalidas');
                return;
            }

            let Reports = {
                "AdminUserID": -1,
                "FRCARDCODE": -1,
                "Usuarios": valVendedor,
                "Tiendas": valTienda,
                "Articulo": valArticuloVentaArt,
                "Linea": valLineaVentaArt,
                "Medida": valMedidaVentaArt,
                "Modelo": valModeloVentaArt,
                "Estatus": valEstatus,
                "Fecha1": dtFechaInicioReportsV,
                "Fecha2": dtFechaFinReportsV
            }

            try {

                $("#modalLoading").modal("show");

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetReportsVentaxArticulo",
                    data: "{'reports' : " + JSON.stringify(Reports) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
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

                            table = $("#tblConsultaVentasxArticuloReport").dataTable({
                                destroy: true,
                                language: spanish,
                                //searching: false,
                                scrollX: true,
                                //scrollCollapse: true,
                                fixedColumns: true,
                                select: false,
                                data: data,
                                columns: [
                                    { title: "ID" },
                                    { title: "Fecha Factura" },
                                    { title: "Tienda" },
                                    { title: "Venta" },
                                    { title: "Vendedor" },
                                    { title: "Cliente" },
                                    { title: "Estatus Venta" },
                                    { title: "Codigo Articulo" },
                                    { title: "Articulo" },
                                    { title: "Lista de Precios" },
                                    { title: "Cantidad" },
                                    { title: "Precio unitario" },
                                    { title: "Descuento" },
                                    { title: "IVA" },
                                    { title: "Total Linea" },
                                    { title: "Total Venta" },
                                    { title: "Monto pagado" },
                                    { title: "Saldo" },
                                    { title: "Push Money" }
                                ],
                                "pageLength": 5,
                                "lengthChange": false,
                                "responsive": true,
                                "rowReorder": {
                                    "selector": 'td:nth-child(2)'
                                },
                                "fixedHeader": {
                                    "header": true
                                },
                                dom: 'Bfrtip',
                                buttons: [
                                    'excel'
                                ]
                            });
                        }
                        else {
                            $(".DTFC_Cloned tbody").empty();
                            $("#tblConsultaVentasxArticuloReport").dataTable().empty();
                        }
                        //Tiempo para cerrar el modal loading
                        setTimeout(function () { $("#modalLoading").modal("hide"); },
                            5000);
                        //Se agrega icono al boton excel
                        $('.buttons-excel').prepend('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        alert("Ha ocurrido un error al generar la consulta del reporte");
                        $("#modalLoading").modal("hide");
                    }
                });
            } catch (ex) {
                alert(ex);
                $("#modalLoading").modal("hide");
            }
        });
    },
    getReportsIngresos: () => {
        $('#btnReportsIngresosFiltro').click(() => {
            let valTienda = $('#cmbTiendaConsult').val().toString();
            let valVendedor = $('#cmbVendedorConsult').val().toString();
            let valTipoPago = $('#cmbTipoPagoConsult').val().toString();
            let dtFechaInicioReportsV = $('#txFechaInicioReportsIngresos').val().toString();
            let dtFechaFinReportsV = $('#txFechaFinReportsIngresos').val().toString();

            let Reports = {
                "AdminUserID": -1,
                "FRCARDCODE": -1,
                "Usuarios": valVendedor,
                "Tiendas": valTienda,
                "TipoPago": valTipoPago,
                "Fecha1": dtFechaInicioReportsV,
                "Fecha2": dtFechaFinReportsV
            }

            try {

                $("#modalLoading").modal("show");

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetReportsIngresos",
                    data: "{'reports' : " + JSON.stringify(Reports) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
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

                            table = $("#tblConsultaIngresosReport").dataTable({
                                destroy: true,
                                language: spanish,
                                //searching: false,
                                scrollX: true,
                                //scrollCollapse: true,
                                fixedColumns: true,
                                select: false,
                                data: data,
                                columns: [
                                    { title: "ID" },
                                    { title: "Fecha" },
                                    { title: "Tienda" },
                                    { title: "Venta" },
                                    { title: "Vendedor" },
                                    { title: "Cliente" },
                                    { title: "Monto" },
                                    { title: "Forma de Pago" },
                                    { title: "Recibo" }
                                ],
                                "pageLength": 5,
                                "lengthChange": false,
                                "responsive": true,
                                "rowReorder": {
                                    "selector": 'td:nth-child(2)'
                                },
                                "fixedHeader": {
                                    "header": true
                                },
                                dom: 'Bfrtip',
                                buttons: [
                                    'excel'
                                ]
                            });
                        }
                        else {
                            $(".DTFC_Cloned tbody").empty();
                            $("#tblConsultaIngresosReport").dataTable().empty();
                        }
                        //Tiempo para cerrar el modal loading
                        setTimeout(function () { $("#modalLoading").modal("hide"); },
                            5000);
                        //Se agrega icono al boton excel
                        $('.buttons-excel').prepend('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        alert("Ha ocurrido un error al generar la consulta del reporte");
                        $("#modalLoading").modal("hide");
                    }
                });
            } catch (ex) {
                alert(ex);
                $("#modalLoading").modal("hide");
            }
        });
    },
    getReportsVTxTienda: () => {
        $('#btnReportsVTxTiendaFiltro').click(() => {
            let valTienda = $('#cmbTiendaConsult').val().toString();
            let dtFechaInicioReportsV = $('#txFechaInicioReportsVTxTienda').val().toString();
            let dtFechaFinReportsV = $('#txFechaFinReportsVTxTienda').val().toString();

            let Reports = {
                "AdminUserID": -1,
                "FRCARDCODE": -1,
                "Tiendas": valTienda,
                "Fecha1": dtFechaInicioReportsV,
                "Fecha2": dtFechaFinReportsV
            }

            try {

                $("#modalLoading").modal("show");

                $('#tblConsultaVTxTiendaReport > thead').empty(); //Se elimina al iniciar

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetReportsVTotalxTienda",
                    data: "{'reports' : " + JSON.stringify(Reports) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        var dataset = JSON.parse(result)
                        var columns = "";
                        if (dataset != null) {
                            var columnDefs = [];
                            //loop for populating column defs

                            //$('#tblConsultaVTxTiendaReport > thead').append('<tr>');
                            for (let k in dataset[0]) {
                                if (columnDefs.indexOf(k) === -1) {
                                    columnDefs.push({ title: dataset[0][k] });
                                }

                                columns = columns + '<th>' + k.toString() + '</th>'; //
                            }

                            $('#tblConsultaVTxTiendaReport > thead').append('<tr>' + columns + '</tr>');  //Se Agrega encabezados a la tabla creada
                            //loop for populating data
                            var data = [];
                            for (let j of dataset) {
                                var arr = [];
                                for (let k in j) {
                                    if (columnDefs.indexOf(k) === -1) {
                                        if (j[k] != null)
                                            arr.push(j[k].toLocaleString('en-US', { style: 'currency', currency: 'USD' }));
                                        else
                                            arr.push(0);
                                    }
                                }
                                data.push(arr);
                            }

                            $('.DTFC_LeftWrapper').empty() //Se elimina contenido de tabla clon de datatable

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

                            var tableT = $("#tblConsultaVTxTiendaReport").dataTable({
                                destroy: true,
                                language: spanish,
                                //searching: false,
                                scrollX: true,
                                //scrollCollapse: true,
                                fixedColumns: true,
                                select: false,
                                data: data,
                                "pageLength": 5,
                                "lengthChange": false,
                                //"responsive": true,
                                "rowReorder": {
                                    "selector": 'td:nth-child(2)'
                                },
                                "fixedHeader": {
                                    "header": true
                                },
                                dom: 'Bfrtip',
                                buttons: [
                                    'excel'
                                ]
                            });
                        }
                        else {
                            $(".DTFC_Cloned tbody").empty();
                            $("#tblConsultaVTxTiendaReport").dataTable().empty();
                        }
                        //Tiempo para cerrar el modal loading
                        setTimeout(function () { $("#modalLoading").modal("hide"); },
                            5000);
                        //Se agrega icono al boton excel
                        $('.buttons-excel').prepend('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        alert("Ha ocurrido un error al generar la consulta del reporte");
                        $("#modalLoading").modal("hide");
                    }
                });
            } catch (ex) {
                alert(ex);
                $("#modalLoading").modal("hide");
            }
        });
    },
    getReportsVTxVendedor: () => {
        $('#btnReportsVTxVendedorFiltro').click(() => {
            let valTienda = $('#cmbTiendaConsult').val().toString();
            let dtFechaInicioReportsV = $('#txFechaInicioReportsVTxVendedor').val().toString();
            let dtFechaFinReportsV = $('#txFechaFinReportsVTxVendedor').val().toString();

            let Reports = {
                "AdminUserID": -1,
                "FRCARDCODE": -1,
                "Tiendas": valTienda,
                "Fecha1": dtFechaInicioReportsV,
                "Fecha2": dtFechaFinReportsV
            }

            try {

                $("#modalLoading").modal("show");

                $('#tblConsultaVTxVendedorReport > thead').empty(); //Se elimina al iniciar

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetReportsVTotalxVendedor",
                    data: "{'reports' : " + JSON.stringify(Reports) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        var dataset = JSON.parse(result)
                        var columns = "";
                        if (dataset != null) {
                            var columnDefs = [];
                            //loop for populating column defs

                            //$('#tblConsultaVTxTiendaReport > thead').append('<tr>');
                            for (let k in dataset[0]) {
                                if (columnDefs.indexOf(k) === -1) {
                                    columnDefs.push({ title: dataset[0][k] });
                                }

                                columns = columns + '<th>' + k.toString() + '</th>'; //
                            }

                            $('#tblConsultaVTxVendedorReport > thead').append('<tr>' + columns + '</tr>');  //Se Agrega encabezados a la tabla creada
                            //loop for populating data
                            var data = [];
                            for (let j of dataset) {
                                var arr = [];
                                for (let k in j) {
                                    if (columnDefs.indexOf(k) === -1) {
                                        if (j[k] != null)
                                            arr.push(j[k].toLocaleString('en-US', { style: 'currency', currency: 'USD' }));
                                        else
                                            arr.push(0);
                                    }
                                }
                                data.push(arr);
                            }

                            $('.DTFC_LeftWrapper').empty() //Se elimina contenido de tabla clon de datatable

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

                            var tableT = $("#tblConsultaVTxVendedorReport").dataTable({
                                destroy: true,
                                language: spanish,
                                //searching: false,
                                scrollX: true,
                                //scrollCollapse: true,
                                fixedColumns: true,
                                select: false,
                                data: data,
                                "pageLength": 5,
                                "lengthChange": false,
                                //"responsive": true,
                                "rowReorder": {
                                    "selector": 'td:nth-child(2)'
                                },
                                "fixedHeader": {
                                    "header": true
                                },
                                dom: 'Bfrtip',
                                buttons: [
                                    'excel'
                                ]
                            });
                        }
                        else {
                            $(".DTFC_Cloned tbody").empty();
                            $("#tblConsultaVTxVendedorReport").dataTable().empty();
                        }
                        //Tiempo para cerrar el modal loading
                        setTimeout(function () { $("#modalLoading").modal("hide"); },
                            5000);
                        //Se agrega icono al boton excel
                        $('.buttons-excel').prepend('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        alert("Ha ocurrido un error al generar la consulta del reporte");
                        $("#modalLoading").modal("hide");
                    }
                });


            } catch (ex) {
                alert(ex);
                $("#modalLoading").modal("hide");
            }
        });
    },
    getReportsTransferencias: () => {
        $('#btnReportsTransferenciasFiltro').click(() => {
            let valTienda = $('#cmbTiendaConsult').val().toString();

            let Reports = {
                "AdminUserID": -1,
                "FRCARDCODE": -1,
                "Tiendas": valTienda
            }

            try {

                $("#modalLoading").modal("show");

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetReportsTransferencia",
                    data: "{'reports' : " + JSON.stringify(Reports) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        let dataset = result;
                        if (dataset != null) {
                            var columnDefs = [];
                            //loop for populating column defs

                            //$('#tblConsultaVTxTiendaReport > thead').append('<tr>');
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
                                        if (j[k] != null)
                                            arr.push(j[k]);
                                        else
                                            arr.push(0);
                                    }
                                }
                                data.push(arr);
                            }

                            $('.DTFC_LeftWrapper').empty() //Se elimina contenido de tabla clon de datatable

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

                            tableT = $("#tblConsultaTransferenciasReport").dataTable({
                                destroy: true,
                                language: spanish,
                                //searching: false,
                                scrollX: true,
                                //scrollCollapse: true,
                                fixedColumns: true,
                                select: false,
                                data: data,
                                columns: [
                                    { title: "ID" },
                                    { title: "Articulo SBO" },
                                    { title: "Descripción" },
                                    { title: "Usuario" },
                                    { title: "Fecha Envío" },
                                    { title: "Cantidad" },
                                    { title: "Tienda Origen" },
                                    { title: "Tienda Destino" },
                                    { title: "FechaDif" },
                                    { title: "Status" },
                                    { title: "Comentarios" },
                                    { 'data': null, title: 'Imprimir', wrap: true, "render": function (item) { return '<div class="btn-group"> <button type="button" id="' + item + '" onclick="Reports.GetImprimirPedidoPDF()" value="0" class="btn btn-success" aria-label="Left Align" data-toggle="modal" data-target="#myModal">  <span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span>Imprimir Envío</button></div>' } },
                                ],
                                "pageLength": 5,
                                "lengthChange": false,
                                //"responsive": true,
                                "rowReorder": {
                                    "selector": 'td:nth-child(2)'
                                },
                                "fixedHeader": {
                                    "header": true
                                },
                                dom: 'Bfrtip',
                                buttons: [
                                    'excel'
                                ]
                            });

                            $('#tblConsultaTransferenciasReport tbody').on('click', 'tr', function () {
                                var data = tableT.api().row(this).data();

                            });
                        }
                        else {
                            $(".DTFC_Cloned tbody").empty();
                            $("#tblConsultaTransferenciasReport").dataTable().empty();
                        }
                        //Tiempo para cerrar el modal loading
                        setTimeout(() => { $("#modalLoading").modal("hide"); },
                            5000);
                        //Se agrega icono al boton excel
                        $('.buttons-excel').prepend('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        alert("Ha ocurrido un error al generar la consulta del reporte");
                        $("#modalLoading").modal("hide");
                    }
                });
            } catch (ex) {
                alert(ex);
                $("#modalLoading").modal("hide");
            }
        });
    },
    getImprimirPedidoPDF: () => {
        $('#tblConsultaTransferenciasReport tbody').on('click', 'tr', (e) => {
            if (e.target.type == "button") {
                let id = e.target.id.split(',')[0];
                let URLactual = window.location;
                let Url = URLactual.toString().split('/');

                let rutaReports = 'http://' + Url[2] + '/Reportes/Download_TrasFerenciasDormimundo_PDF?IdAbono=' + id;
                window.location.href = rutaReports.replace("''", "");
            }
        });
    },
    getReportsVTxCompras: () => {
        $('#btnReportsComprasFiltro').click(() => {
            let valTienda = $('#cmbTiendaConsult').val().toString();
            let dtFechaInicioReportsV = $('#txFechaInicioReportsCompras').val().toString();
            let dtFechaFinReportsV = $('#txFechaFinReportsCompras').val().toString();

            let Reports = {
                "AdminUserID": -1,
                "FRCARDCODE": -1,
                "Tiendas": valTienda,
                "Fecha1": dtFechaInicioReportsV,
                "Fecha2": dtFechaFinReportsV
            }

            try {

                $("#modalLoading").modal("show");

                $('#tblConsultaComprasReport > thead').empty(); //Se elimina al iniciar

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetReportscompras",
                    data: "{'reports' : " + JSON.stringify(Reports) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        var dataset = result
                        var columns = "";
                        if (dataset != null) {
                            var columnDefs = [];
                            //loop for populating column defs

                            //$('#tblConsultaVTxTiendaReport > thead').append('<tr>');
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
                                        if (j[k] != null)
                                            arr.push(j[k]);
                                        else
                                            arr.push(0);
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

                            var tableT = $("#tblConsultaComprasReport").dataTable({
                                destroy: true,
                                language: spanish,
                                //searching: false,
                                scrollX: true,
                                //scrollCollapse: true,
                                fixedColumns: true,
                                select: false,
                                data: data,
                                columns: [
                                    { title: "ID Entrada" },
                                    { title: "Documento SAP" },
                                    { title: "Artículo" },
                                    { title: "Cantidad" },
                                    { title: "Linea SBO" },
                                    { title: "Fecha SBO" },
                                    { title: "Fecha Ent. Web" },
                                    { title: "SNSBO" },
                                    { title: "Usuario" },
                                    { title: "Tienda" },
                                    { title: "FolioPOS" }
                                ],
                                "lengthChange": false,
                                //"responsive": true,
                                "rowReorder": {
                                    "selector": 'td:nth-child(2)'
                                },
                                "fixedHeader": {
                                    "header": true
                                },
                                dom: 'Bfrtip',
                                buttons: [
                                    'excel'
                                ]
                            });
                        }
                        else {
                            $(".DTFC_Cloned tbody").empty();
                            $("#tblConsultaComprasReport").dataTable().empty();
                        }
                        //Tiempo para cerrar el modal loading
                        setTimeout(function () { $("#modalLoading").modal("hide"); },
                            5000);
                        //Se agrega icono al boton excel
                        $('.buttons-excel').prepend('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        alert("Ha ocurrido un error al generar la consulta del reporte");
                        $("#modalLoading").modal("hide");
                    }
                });
            } catch (ex) {
                alert(ex);
                $("#modalLoading").modal("hide");
            }
        });
    },
    getReportsKardex: () => {
        $('#btnReportsKardexFiltro').click(() => {
            let valTienda = $('#cmbTiendaConsultKardex option:selected').val().toString();
            let dtFechaInicioReportsV = $('#txFechaIKardex').val().toString();
            let dtFechaFinReportsV = $('#txFechaFKardex').val().toString();
            //let cmbArticuloKardex = $('#cmbArticuloKardex').val().toString();
            let tienda = $('#cmbTiendaConsultKardex').find('option:selected').text();

            let Reports = {
                "Whs": valTienda,
                "Fecha1": dtFechaInicioReportsV,
                "Fecha2": dtFechaFinReportsV,
                "Store": tienda
            }

            try {

                $("#modalLoading").modal("show");

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetReportsKardex",
                    data: "{'reports' : " + JSON.stringify(Reports) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        var dataset = result
                        var columns = "";
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
                                        if (j[k] != null)
                                            arr.push(j[k]);
                                        else
                                            arr.push(0);
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

                            var tableT = $("#tblConsultaKardexReport").dataTable({
                                destroy: true,
                                language: spanish,
                                //searching: false,
                                scrollX: true,
                                //scrollCollapse: true,
                                paging: false,
                                fixedColumns: true,
                                select: false,
                                data: data,
                                columns: [
                                    { title: "Id" },
                                    { title: "TIPO MOV" },
                                    { title: "FECHA MOVIMIENTO" },
                                    { title: "TIENDA" },
                                    { title: "DESCRIPCIÓN ARTÍCULO" },
                                    { title: "CANTIDAD" },
                                    { title: "CANTIDAD <br /> ACUMULADA" },
                                    { title: "REFERENCIA" }
                                ],
                                "lengthChange": false,
                                //"responsive": true,
                                //"rowReorder": {
                                //    "selector": 'td:nth-child(2)'
                                //},
                                "order": [[0, "asc"]],
                                "fixedHeader": {
                                    "header": true
                                },
                                dom: 'Bfrtip',
                                buttons: [
                                    'excel'
                                ],
                                fixedColumns: true
                            });
                        }
                        else {
                            $(".DTFC_Cloned tbody").empty();
                            $("#tblConsultaKardexReport").dataTable().empty();
                        }
                        //Tiempo para cerrar el modal loading
                        setTimeout(function () { $("#modalLoading").modal("hide"); },
                            5000);
                        //Se agrega icono al boton excel
                        $('.buttons-excel').prepend('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        alert("Ha ocurrido un error al generar la consulta del reporte");
                        $("#modalLoading").modal("hide");
                    }
                });
            } catch (ex) {
                alert(ex);
                $("#modalLoading").modal("hide");
            }
        });
    },
    getFranquicias: () => {
        $('#cmbTipoReporte').change(() => {
            if ($('#cmbTipoReporte').val() == 0) {
                $('#cmbPushMoneyFranquicia').prop('disabled', true);
                $('#cmbPushMoneyVendedor').prop('disabled', true);
            }
            else {
                $('#cmbPushMoneyFranquicia').removeAttr('disabled');
                $('#cmbPushMoneyVendedor').removeAttr('disabled');
                try {
                    let Reports = {
                        "AdminUserID": -1
                    }

                    $.ajax({
                        type: "POST",
                        url: "/Reportes/GetFranquiciasPushMoney",
                        data: "{'reports' : " + JSON.stringify(Reports) + "}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: (result) => {
                            let data = result;
                            if (data != null) {
                                $('#cmbPushMoneyFranquicia').empty();
                                $('#cmbPushMoneyFranquicia').append("<option value='0'>Seleccione Franquicia</option>");
                                for (let item in data) {
                                    $('#cmbPushMoneyFranquicia').append("<option value='" + data[item].CardCode + "'>" + data[item].CardName + "</option>");
                                }
                            }
                        },
                        error: (xhr) => {
                            console.log(xhr.responseText);
                            alert("Ha ocurrido un error al generar la consulta");
                        }
                    });
                } catch (ex) {
                    alert(ex);
                }
            }
        });
    },
    getVendedoresPush: () => {
        $('#cmbPushMoneyFranquicia').change(() => {
            try {
                let UsersView = {
                    "Franquicia": $('#cmbPushMoneyFranquicia').val().toString()
                }

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetVendedorPushMoney",
                    data: "{'usersView' : " + JSON.stringify(UsersView) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        let data = result;
                        if (data != null) {
                            $('#cmbPushMoneyVendedor').empty();
                            $('#cmbPushMoneyVendedor').append("<option value='0'>Seleccione Vendedor</option>");
                            for (let item in data) {
                                $('#cmbPushMoneyVendedor').append("<option value='" + data[item].AdminUserID + "'>" + data[item].UserName + "</option>");
                            }
                        }
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        alert("Ha ocurrido un error al generar la consulta");
                    }
                });
            } catch (ex) {
                alert(ex);
            }
        });
    },
    getReportsPushMoney: () => {
        $('#btnReportsPushMoneyFiltro').click(() => {
            let valTipoReporte = $('#cmbTipoReporte').val().toString();
            let valFranquicia = $('#cmbPushMoneyFranquicia').val().toString();
            let valVendedor = $('#cmbPushMoneyVendedor').val().toString();
            let dtFechaInicioReportsV = $('#txFechaIKardex').val().toString();
            let dtFechaFinReportsV = $('#txFechaFKardex').val().toString();

            let usersView = {
                "TipoReporte": valTipoReporte,
                "FRCARDCODE": valFranquicia,
                "Vendedor": valVendedor,
                "Fecha1": dtFechaInicioReportsV,
                "Fecha2": dtFechaFinReportsV
            }

            if (valTipoReporte == '0') {
                $("#modalLoading").modal("show");

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetReportsPushMoneyxGralFranquicia",
                    data: "{'usersView' : " + JSON.stringify(usersView) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        var dataset = result

                        $('#tblConsultaPushMoneyReportF').parents('div.dataTables_wrapper').first().hide();
                        $('#tblConsultaPushMoneyReportT_wrapper').hide();

                        if (dataset.length == 0) {
                            alert('Los Filtros no generaron un reporte');

                            setTimeout(function () { $("#modalLoading").modal("hide"); },
                                5000);
                        }
                        else {
                            if (dataset != null) {
                                $('#tblConsultaPushMoneyReportGralFranquicia_wrapper').find('table').first().show();
                                $('#tblConsultaPushMoneyReportGralFranquicia').show();

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

                                table = $("#tblConsultaPushMoneyReportGralFranquicia").dataTable({
                                    destroy: true,
                                    "language": {
                                        "decimal": ".",//separador decimales
                                        "thousands": ","//Separador miles
                                    },
                                    scrollY: "300px",
                                    scrollCollapse: true,
                                    fixedColumns: {
                                        leftColumns: 1,
                                        rightColumns: 1
                                    },
                                    //searching: false,
                                    //scrollCollapse: true,
                                    fixedColumns: true,
                                    select: false,
                                    data: data,

                                    columns: [
                                        { title: "Card. Code" },
                                        { title: "Franquicia" },
                                        { title: "Subtotal" },
                                        { title: "Iva" },
                                        { title: "TotalPush" }
                                    ],
                                    "pageLength": 5,
                                    "lengthChange": false,
                                    "responsive": true,
                                    "rowReorder": {
                                        "selector": 'td:nth-child(2)'
                                    },
                                    "fixedHeader": {
                                        "header": true
                                    },
                                    dom: 'Bfrtip',
                                    buttons: [
                                        'excel'
                                    ]
                                });
                            }
                            else {
                                $(".DTFC_Cloned tbody").empty();
                                $("#tblConsultaPushMoneyReportGralFranquicia").dataTable().empty();
                            }
                            //Tiempo para cerrar el modal loading
                            setTimeout(function () { $("#modalLoading").modal("hide"); },
                                5000);
                            //Se agrega icono al boton excel
                            //$('.buttons-excel').prepend('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                        }
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        alert("Ha ocurrido un error al generar la consulta del reporte de ventas");
                        $("#modalLoading").modal("hide");
                    }
                });
            }
            else {
                if (valVendedor == '0') {
                    $("#modalLoading").modal("show");

                    $.ajax({
                        type: "POST",
                        url: "/Reportes/GetReportsPushMoneyxVendedor",
                        data: "{'usersView' : " + JSON.stringify(usersView) + "}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: (result) => {
                            var dataset = result

                            $('#tblConsultaPushMoneyReportF').parents('div.dataTables_wrapper').first().hide();
                            $('#tblConsultaPushMoneyReportGralFranquicia').parents('div.dataTables_wrapper').first().hide();

                            if (dataset.length == 0) {
                                $("#modalLoading").modal("hide");
                                alert('Los Filtros no generaron un reporte');
                            }
                            else {
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
                                            if (k.toString() == 'PushMoney' || k.toString() == 'Total') {
                                                if (columnDefs.indexOf(k) === -1) {
                                                    if (j[k] != null)
                                                        arr.push(j[k].toLocaleString('en-US', { style: 'currency', currency: 'USD' }));
                                                    else {
                                                        arr.push(0);
                                                    }
                                                }
                                                else
                                                    arr.push(0);
                                            }
                                            else {
                                                if (columnDefs.indexOf(k) === -1) {
                                                    arr.push(j[k]);
                                                }
                                                else
                                                    arr.push(0);
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

                                    table = $("#tblConsultaPushMoneyReportT").dataTable({
                                        destroy: true,
                                        "language": {
                                            "decimal": ".",//separador decimales
                                            "thousands": ","//Separador miles
                                        },
                                        scrollY: "300px",
                                        scrollCollapse: true,
                                        fixedColumns: {
                                            leftColumns: 1,
                                            rightColumns: 1
                                        },
                                        //searching: false,
                                        //scrollCollapse: true,
                                        fixedColumns: true,
                                        select: false,
                                        data: data,

                                        columns: [
                                            { title: "ID" },
                                            { title: "Nombre" },
                                            { title: "Total Vendedor" }
                                        ],
                                        "pageLength": 5,
                                        "lengthChange": false,
                                        "responsive": true,
                                        "rowReorder": {
                                            "selector": 'td:nth-child(2)'
                                        },
                                        "fixedHeader": {
                                            "header": true
                                        },
                                        dom: 'Bfrtip',
                                        buttons: [
                                            'excel'
                                        ]
                                    });
                                    $("#modalLoading").modal("hide");
                                }
                                else {
                                    $(".DTFC_Cloned tbody").empty();
                                    $("#tblConsultaPushMoneyReportT").dataTable().empty();
                                }
                                //Tiempo para cerrar el modal loading
                                setTimeout(function () { $("#modalLoading").modal("hide"); },
                                    5000);
                                //Se agrega icono al boton excel
                                // $('.buttons-excel').prepend('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');

                                table.columns.adjust().draw();
                            }
                        },
                        error: (xhr) => {
                            console.log(xhr.responseText);
                            alert("Ha ocurrido un error al generar la consulta del reporte de ventas");
                            $("#modalLoading").modal("hide");
                        }
                    });
                }
                else {
                    try {

                        $("#modalLoading").modal("show");

                        //$('#tblConsultaPushMoneyReportF > thead').empty(); //Se elimina al iniciar

                        $.ajax({
                            type: "POST",
                            url: "/Reportes/GetReportsPushMoney",
                            data: "{'usersView' : " + JSON.stringify(usersView) + "}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: (result) => {
                                var dataset = result
                                var columns = "";

                                $('#tblConsultaPushMoneyReportGralFranquicia').hide();
                                $('#tblConsultaPushMoneyReportT_wrapper').hide();

                                if (dataset.length == 0) {
                                    $("#modalLoading").modal("hide");
                                    alert('Los Filtros no generaron un reporte');
                                }
                                else
                                    if (dataset != null) {
                                        var columnDefs = [];
                                        //loop for populating column defs

                                        //$('#tblConsultaVTxTiendaReport > thead').append('<tr>');
                                        for (let k in dataset[0]) {
                                            if (columnDefs.indexOf(k) === -1) {
                                                columnDefs.push({ title: dataset[0][k] });
                                            }

                                            //columns = columns + '<th>' + k.toString() + '</th>'; //
                                        }

                                        //$('#tblConsultaPushMoneyReportT > thead').append('<tr>' + columns + '</tr>');  //Se Agrega encabezados a la tabla creada
                                        //loop for populating data
                                        var data = [];
                                        for (let j of dataset) {
                                            var arr = [];
                                            for (let k in j) {
                                                if (k.toString() == 'PushMoney' || k.toString() == 'Total') {
                                                    if (columnDefs.indexOf(k) === -1) {
                                                        arr.push(j[k].toLocaleString('en-US', { style: 'currency', currency: 'USD' }));
                                                    }
                                                    else
                                                        arr.push(0);
                                                }
                                                else {
                                                    if (columnDefs.indexOf(k) === -1) {
                                                        arr.push(j[k]);
                                                    }
                                                    else
                                                        arr.push(0);
                                                }
                                            }
                                            data.push(arr);
                                        }

                                        //$('.DTFC_LeftWrapper').empty() //Se elimina contenido de tabla clon de datatable

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

                                        var tableT = $("#tblConsultaPushMoneyReportF").dataTable({
                                            destroy: true,
                                            language: spanish,
                                            //searching: false,
                                            scrollX: true,
                                            //scrollCollapse: true,
                                            fixedColumns: true,
                                            select: false,
                                            data: data,
                                            columns: [
                                                { title: "Tienda" },
                                                { title: "Franquicia" },
                                                { title: "Fecha Venta" },
                                                { title: "Factura" },
                                                { title: "Item Name" },
                                                { title: "Push Money" },
                                                { title: "Cantidad" },
                                                { title: "Total" }
                                            ],
                                            "pageLength": 5,
                                            "lengthChange": false,
                                            //"responsive": true,
                                            "fixedHeader": {
                                                "header": true
                                            },
                                            dom: 'Bfrtip',
                                            buttons: [
                                                'excel'
                                            ],
                                            "bSort": false
                                        });

                                        setTimeout(function () { $("#modalLoading").modal("hide"); },
                                            5000);
                                    }
                                    else {
                                        $(".DTFC_Cloned tbody").empty();
                                        $("#tblConsultaPushMoneyReportF").dataTable().empty();


                                        $("#modalLoading").modal("hide");

                                    }
                                //Tiempo para cerrar el modal loading
                                setTimeout(function () { $("#modalLoading").modal("hide"); },
                                    5000);
                                //Se agrega icono al boton excel
                                //$('.buttons-excel').prepend('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                            },
                            error: (xhr) => {
                                console.log(xhr.responseText);
                                alert("Ha ocurrido un error al generar la consulta del reporte");
                                $("#modalLoading").modal("hide");
                            }
                        });
                    } catch (ex) {
                        alert(ex);
                        $("#modalLoading").modal("hide");
                    }
                }
            }
        });
    },
    getReportsFactPagos: () => {
        $('#btnReportsFactPagosFiltro').click(() => {
            let valTienda = $('#cmbTiendaConsult').val();
            let valVendedor = $('#cmbVendedorConsult').val();
            let dtFechaInicioReportsV = $('#txFechaIReportsFactPagos').val().toString();
            let dtFechaFinReportsV = $('#txFechaFReportsFactPagos').val().toString();

            let Reports = {
                "AdminUserID": -1,
                "FRCARDCODE": -1,
                "Tiendas": valTienda,
                "Vendedor": valVendedor,
                "Fecha1": dtFechaInicioReportsV,
                "Fecha2": dtFechaFinReportsV
            }

            try {

                $("#modalLoading").modal("show");

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetReportsFactPagos",
                    data: "{'reports' : " + JSON.stringify(Reports) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        var dataset = result
                        var columns = "";
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
                                        if (j[k] != null)
                                            arr.push(j[k]);
                                        else
                                            arr.push(0);
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

                            var tableT = $("#tblConsultaFactPagosReport").dataTable({
                                destroy: true,
                                language: spanish,
                                //searching: false,
                                scrollX: true,
                                //scrollCollapse: true,
                                fixedColumns: true,
                                select: false,
                                data: data,
                                columns: [
                                    { title: "Fecha Pago" },
                                    { title: "Fecha Factura" },
                                    { title: "Tienda" },
                                    { title: "Venta" },
                                    { title: "Folio Pago" },
                                    { title: "Forma Pago" },
                                    { title: "Vendedor" },
                                    { title: "Cliente" },
                                    { title: "Estatus Pago" },
                                    { title: "Total Venta" },
                                    { title: "Monto Pagado" },
                                    { title: "Monto Pagado" },
                                    { title: "Estatus Pagado" }
                                ],
                                "lengthChange": false,
                                //"responsive": true,
                                "rowReorder": {
                                    "selector": 'td:nth-child(2)'
                                },
                                "fixedHeader": {
                                    "header": true
                                },
                                dom: 'Bfrtip',
                                buttons: [
                                    'excel'
                                ]
                            });
                        }
                        else {
                            $(".DTFC_Cloned tbody").empty();
                            $("#tblConsultaFactPagosReport").dataTable().empty();
                        }
                        //Tiempo para cerrar el modal loading
                        setTimeout(function () { $("#modalLoading").modal("hide"); },
                            5000);
                        //Se agrega icono al boton excel
                        $('.buttons-excel').prepend('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        alert("Ha ocurrido un error al generar la consulta del reporte");
                        $("#modalLoading").modal("hide");
                    }
                });
            } catch (ex) {
                alert(ex);
                $("#modalLoading").modal("hide");
            }
        });
    },
    setLocalStorage: () => {
        $("#TiendaSesion").find('p').remove();
        $("#UsuarioSesion").find('p').remove();

        if ($("#TiendaSesion").children.length == 2) {
            let Tienda = sessionStorage.getItem('Tienda');
            let Usuario = sessionStorage.getItem('Usuario')
            $("#TiendaSesion").html(Tienda);
            $("#UsuarioSesion").append(Usuario);
        }
        else {
            $("#TiendaSesion").find('p').remove();
            $("#UsuarioSesion").find('p').remove();
        }
    },
    getReportsNomina: () => {
        $('#btnReportsNomina').click(() => {
            let valTienda = $('#cmbTiendaConsult').val().toString();
            let valVendedor = $('#cmbVendedorConsult').val().toString();
            let dtFechaInicioReportsV = $('#txFechaIReportsNomina').val().toString();
            let dtFechaFinReportsV = $('#txFechaFReportsNomina').val().toString();

            let Reports = {
                "AdminUserID": -1,
                "FRCARDCODE": -1,
                "Usuarios": valVendedor,
                "Tiendas": valTienda,
                "Fecha1": dtFechaInicioReportsV,
                "Fecha2": dtFechaFinReportsV
            }
            try {
                $("#modalLoading").modal("show");

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetReportsNomina",
                    data: "{'reports' : " + JSON.stringify(Reports) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        var dataset = JSON.parse(result)
                        var columns = "";
                        if (dataset.length > 0) {
                            var columnDefs = [];
                            //loop for populating column defs

                            //$('#tblConsultaVTxTiendaReport > thead').append('<tr>');
                            let columna = '';
                            for (let k in dataset[0]) {

                                if (columnDefs.indexOf(k) === -1) {
                                    columnDefs.push({ title: dataset[0][k] });
                                }
                                columna = k.toString();
                                if (columna == 'FechaCFDI') columna = 'Fecha CFDI';
                                if (columna == 'FechaCompletado') columna = 'Fecha <br> Completado';
                                if (columna == 'Tipo_Documento') columna = 'Tipo <br> Documento';
                                if (columna == 'Estatus_Venta') columna = 'Estatus <br> Venta';
                                if (columna == 'Codigo_Articulo') columna = 'Codigo <br> Artículo';
                                if (columna == 'Lista_de_Precios') columna = 'Lista de Precios';
                                if (columna == 'Precio_unitario') columna = 'Precio unitario';
                                if (columna == 'Total_Linea') columna = 'Total Linea';
                                if (columna == 'Total_Venta') columna = 'Total Venta';
                                if (columna == 'Monto_pagado') columna = 'Monto pagado';
                                if (columna == 'Push_Money') columna = 'Push Money';
                                if (columna == 'Cliente_SAP') columna = 'Cliente SAP';


                                columns = columns + '<th>' + columna + '</th>'; //
                            }

                            $('#tblConsultaNominaReport > thead').append('<tr>' + columns + '</tr>');  //Se Agrega encabezados a la tabla creada
                            //loop for populating data
                            var data = [];
                            for (let j of dataset) {
                                var arr = [];
                                for (let k in j) {
                                    if (columnDefs.indexOf(k) === -1) {
                                        if (j[k] != null)
                                            if (j[k] == '1900-01-01T00:00:00') {
                                                arr.push(' ');
                                            }
                                            else
                                                if (k == 2 && k == 3 && k == 4) {
                                                    arr.push(j[k]);
                                                }
                                                else
                                                    if (k == 'Precio_unitario' ||
                                                        k == "Descuento" ||
                                                        k == "subTotal" ||
                                                        k == "Total_Linea" ||
                                                        k == "Total_Venta" ||
                                                        k == "Monto_pagado" ||
                                                        k == "Total_Venta" ||
                                                        k == "Saldo" ||
                                                        k == "IVA" ||
                                                        k == "Push_Money")
                                                        arr.push(j[k].toLocaleString('en-US', { style: 'currency', currency: 'USD' }));
                                                    else
                                                        arr.push(j[k]);
                                        else
                                            arr.push(0);
                                    }
                                }
                                data.push(arr);
                            }

                            $('.DTFC_LeftWrapper').empty() //Se elimina contenido de tabla clon de datatable

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

                            var tableT = $("#tblConsultaNominaReport").dataTable({
                                destroy: true,
                                language: spanish,
                                "order": [[3, "desc"]],
                                //searching: false,
                                scrollX: true,
                                //scrollCollapse: true,
                                fixedColumns: true,
                                select: false,
                                data: data,
                                "pageLength": 20,
                                "lengthChange": false,
                                //"responsive": true,
                                "rowReorder": {
                                    "selector": 'td:nth-child(2)'
                                },
                                "fixedHeader": {
                                    "header": true
                                },
                                dom: 'Bfrtip',
                                buttons: [
                                    'excel'
                                ],
                                columnDefs: [
                                    { width: 200, targets: 0 }
                                ]
                            });
                        }
                        else {
                            alert('Los filtros no genero un reporte')
                            $(".DTFC_Cloned tbody").empty();
                            $("#tblConsultaNominaReport").dataTable().empty();
                        }
                        //Tiempo para cerrar el modal loading
                        setTimeout(function () { $("#modalLoading").modal("hide"); },
                            5000);
                        //Se agrega icono al boton excel
                        $('.buttons-excel').prepend('<i class="fa fa-file-excel-o" aria-hidden="true"></i>');
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        alert("Ha ocurrido un error al generar la consulta del reporte");
                        $("#modalLoading").modal("hide");
                    }
                });
            } catch (ex) {
                alert(ex);
                $("#modalLoading").modal("hide");
            }
        });
    },
    getReportPayback: () => {
        $('#btnReportFilterPayback').click(() => {
            let valTienda = $('#cmbTiendaConsult').val().toString();
            let valVendedor = $('#cmbVendedorConsult').val().toString();
            let dtFechaInicioReportsV = $('#idDatein').val().toString();
            let dtFechaFinReportsV = $('#idDateFin').val().toString();

            let Reports = {
                "AdminUserID": -1,
                "FRCARDCODE": -1,
                "Usuarios": valVendedor,
                "Tiendas": valTienda,
                "Fecha1": dtFechaInicioReportsV,
                "Fecha2": dtFechaFinReportsV
            }

            try {

                $("#modalLoading").modal("show");

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetReportsPayback",
                    data: "{'reports' : " + JSON.stringify(Reports) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        var dataset = result;
                        var result1 = JSON.parse(result);
                        if (dataset != null) {

                            var spanish = {
                                "processing": "Procesando...",
                                "lengthMenu": "Mostrar _MENU_  registros",
                                "zeroRecords": "No se encontraron resultados",
                                "emptyTable": "Ningún dato disponible en esta tabla",
                                "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                                "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                                "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                                "search": "Buscar :",
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

                            table = $("#tblReportsPayback").dataTable({
                                destroy: true,

                                language: spanish,
                                //searching: false,
                                //scrollX: true,
                                iDisplayLength: 15,
                                //scrollCollapse: true,
                                //select: false,
                                data: result1,
                                columns: [
                                    { title: "ID", data: "ID" },
                                    { title: "Fecha Pedido", data: "FechaPedido" },
                                    { title: "Fecha Factura", data: "FechaFactura" },
                                    { title: "Fecha Completado", data: "FechaCompletado" },
                                    { title: "Tienda", data: "Tienda" },
                                    { title: "Venta", data: "Venta" },
                                    { title: "Vendedor", data: "Vendedor" },
                                    { title: "Cliente", data: "Cliente" },
                                    { title: "StatusVenta", data: "EstatusVenta" },
                                    { title: "Facturado", data: "Facturado" },
                                    { title: "Total Venta", data: "Total_Venta" },
                                    { title: "Monto pagado", data: "Monto_pagado" },
                                    { title: "Saldo", data: "Saldo" },
                                    { title: "Estatus Factura", data: "Estatus_Factura" },
                                    { title: "Fecha de transaccion", data: "FechaTrans" },
                                    { title: "Numero de Recibo", data: "NumeroRecibo" },
                                    { title: "Tipo de transacción", data: "TipoTransaccion" },
                                    { title: "Pago con Puntos", data: "PagadoPuntos" },
                                    { title: "Pago en $", data: "PagadoPesos" },
                                    { title: "Acumulado en puntos", data: "AbonadoPuntos" },
                                    { title: "Acumulado en $", data: "AbonadoPesos" },
                                    { title: "Abonado en puntos", data: "AcumuladoPuntos" },
                                    { title: "Abonado en $", data: "AcumuladoPesos" },
                                    { title: "Conciliado", data: "Conciliado" },
                                    { title: "Fecha Payback", data: "FechaPayback" },
                                    { title: "Fecha Conciliación", data: "FechaConciliacion" },
                                    { title: "Socio", data: "Socio" },                                    
                                    { title: "Archivo", data: "NombreArchivo" },
                                    { title: "Status Transacción", data: "EstatusTransaccion" },
                                    { title: "Error en transacción", data: "ErrorMessage" }
                                ],
                                "responsive": true,
                                "rowReorder": {
                                    "selector": 'td:nth-child(2)'
                                },
                                "processing": true,
                                //"pageLength": 40,
                                dom: 'Bfrtip',
                                buttons: [
                                    'excel'
                                ] 
                            });
                        }
                        else {
                            $(".DTFC_Cloned tbody").empty();
                            $("#tblReportsPayback").dataTable().empty();
                        }
                        //Tiempo para cerrar el modal loading
                        setTimeout(function () { $("#modalLoading").modal("hide"); },
                            5000);
                        //Se agrega icono al boton excel
                        $('.buttons-excel').prepend('<i class="fa fa-file-excel-o center text-center" aria-hidden="true"></i>');
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        alert("Ha ocurrido un error al generar la consulta del reporte");
                        $("#modalLoading").modal("hide");
                    }
                });
            } catch (ex) {
                alert(ex);
                $("#modalLoading").modal("hide");
            }
        });

    },
    getReportDesc: () => {
        $('#btnReportFilterDesc').click(() => {
            let valTienda = $('#cmbTiendaConsult').val().toString();
            let valVendedor = $('#cmbVendedorConsult').val().toString();
            let dtFechaInicioReportsV = $('#idFechaInD').val().toString();
            let dtFechaFinReportsV = $('#idFechaFinD').val().toString();

            if (dtFechaInicioReportsV == '' && dtFechaFinReportsV == '') {
                toastr.warning('Debe seleccionar fecha de inicio y fecha fin para el reporte.', 'Fechas Invalidas');
                return;
            }
            let Reports = {
                "AdminUserID":-1,
                "FRCARDCODE":-1,
                "Usuarios":valVendedor,
                "Tiendas":valTienda,
                "Fecha1": dtFechaInicioReportsV,
                "Fecha2": dtFechaFinReportsV
            }

            try {

                $("#modalLoading").modal("show");

                $.ajax({
                    type: "POST",
                    url: "/Reportes/GetReportsDesc",
                    data: "{'reports' : " + JSON.stringify(Reports) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        var dataset = result;
                        var result1 = JSON.parse(result);
                        if (dataset != null) {

                            var spanish = {
                                "processing": "Procesando...",
                                "lengthMenu": "Mostrar _MENU_  registros",
                                "zeroRecords": "No se encontraron resultados",
                                "emptyTable": "Ningún dato disponible en esta tabla",
                                "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                                "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                                "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                                "search": "Buscar :",
                                "infoThousands": ",",
                                "loadingRecords": "Cargando...",
                                "oPaginate": {
                                    "sFirst": "Primero",
                                    "sLast": "Último",
                                    "sNext": "Siguiente",
                                    "sPrevious": "Anterior"
                                },
                                " oAria ": {
                                    " sSortDescending ": ": Activar para ordenar la columna de manera descendente ",
                                    " sSortAscending ": ": Activar para ordenar la columna de manera ascendente "
                                }
                            }

                            table = $("#tblReportDesc").dataTable({
                                destroy: true,

                                language: spanish,
                                //searching: false,
                                //scrollX: true,
                                iDisplayLength: 15,
                                //scrollCollapse: true,
                                //select: false,
                                data: result1,
                                columns: [
                                    { title: "ID", data: "ID" },
                                    { title: "Vendedor", data: "Vendedor" },
                                    { title: "Tienda", data: "Tienda" },
                                    { title: "Articulo", data: "Articulo" },
                                    { title: "Fecha", data: "FechaFactura" },
                                    { title: "Descuento", data: "Descuento" },
                                    { title: "Codigo", data: "Codigo" },
                                    { title: "Utilizado", data: "Utilizado" },
                                    { title: "Cliente", data: "Cliente" },
                                    { title: "Observaciones", data: "Observaciones" },
                                    { title: "Descuento Modificado", data: "DescuentoModificado" },
                                    { title: "Precio Lista", data: "PrecioFDO" },
                                    { title: "Precio SMU", data: "PrecioSMU" },
                                    { title: "Margen", data: "Margen" }
                                ],
                                "responsive": true,
                                "rowReorder": {
                                    "selector": 'td:nth-child(2)'
                                },
                                "processing": true,
                                "order": [[0, "desc"]],
                                //"pageLength": 40,
                                dom: 'Bfrtip',
                                buttons: [
                                    'excel'
                                ]
                            });
                        }
                        else {
                            $(".DTFC_Cloned tbody").empty();
                            $("#tblReportsPayback").dataTable().empty();
                        }
                        //Tiempo para cerrar el modal loading
                        setTimeout(function () { $("#modalLoading").modal("hide"); },
                            5000);
                        //Se agrega icono al boton excel
                        $('.buttons-excel').prepend('<i class="fa fa-file-excel-o center text-center" aria-hidden="true"></i>');
                    },
                    error: (xhr) => {
                        console.log(xhr.responseText);
                        toastr.error("Ha ocurrido un error al generar la consulta del reporte");
                        $("#modalLoading").modal("hide");
                    }
                });
            } catch (ex) {
                alert(ex);
                $("#modalLoading").modal("hide");
            }
        });
    }

};

$(document).ready(() => {
    Reports.getReportsVentas();
    Reports.getReportsFacturacion();
    Reports.getReportsClienteAvisa();
    Reports.getReportsTotalVenta();
    Reports.getReportsVentaxArticulo();
    Reports.getReportsIngresos();
    Reports.getReportsVTxTienda();
    Reports.getReportsVTxVendedor();
    Reports.getReportsTransferencias();
    Reports.getImprimirPedidoPDF();
    Reports.getReportsVTxCompras();
    Reports.getReportsKardex();
    Reports.getFranquicias();
    Reports.getVendedoresPush();
    Reports.getReportsPushMoney();
    Reports.getReportsFactPagos();
    Reports.setLocalStorage();
    Reports.getReportsNomina();
    Reports.getReportPayback();
    Reports.getReportDesc();


});

function Validate() {
    var SelectFile = ($("#fileUpload"))[0].files[0];
    if (SelectFile == undefined) {
        $("#fileUpload").css('background-color', 'bisque');
        $("#fileUpload").css('color', 'purple');
    } else {
        $("#fileUpload").css('background-color', 'green');
        $("#fileUpload").css('color', 'white');
    }
}
//$(document).ready(function () {
//    $(document).bind("contextmenu", function (e) {
//        return false;
//    });
//});

//$(document).keydown(function (event) {
//    if (event.keyCode == 123) { // Prevent F12
//        return false;
//    } else if (event.ctrlKey && event.shiftKey && event.keyCode == 73) { // Prevent Ctrl+Shift+I        
//        return false;
//    }
//});