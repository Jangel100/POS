function addToDataListCcorta(data) {
    data.forEach(elem => {
        option = document.createElement("option");
        option.value = elem.clavecorta;
        option.setAttribute("data-code", elem.code);
        option.setAttribute("data-name", elem.name);
        datalistOptions.append(option);
    });
}
function Clear() {
    event.preventDefault();
    //OnchangeddlModeloCveC("Linea");
    document.getElementById('cantidad').disabled = true;
    document.getElementById('addarticle').disabled = true;
    $("#item").val("");
    $("#code").val("");
    $("#cantidad").val("1");
    $("#comentarios").val("");
    $("#modelocode").val("");
    $('#datalistmodeloscom').val("");
    var Articulo = GetJsonArtculos();
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
    var idCheckLine = document.getElementById('idCheckLine');
    var idCheckDesc = document.getElementById('idCheckDesc');
    idCheckLine.checked = true;
    idCheckDesc.checked = false;
}

function OnchangeddlModeloCveC(tipo) {
    $.ajax({
        type: "POST",
        url: '/Compras/GetSelectOnchangeCveC',
        data: "{ 'tipo': '" + tipo + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result != null) {
                var lsLinea = JSON.parse(result);
                $("#datalistOptions")
                    .empty();
                var ccortamodelo = $('#ccortamodelo').val();
                if (ccortamodelo == "Modelo") {
                    addToDataList(lsLinea);
                }
                else {
                    addToDataListCcorta(lsLinea);
                }
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

function CheckLineCom() {

    var idCheckDesc = document.getElementById('idCheckDesc');
    var idCheckLine = document.getElementById('idCheckLine');
    if (idCheckLine.checked) {
        OnchangeddlModeloCveC("Linea");
        Clear();
        idCheckDesc.checked = false;
        idCheckLine.checked = true;
        $("#articuloen").val("Linea");
    } else {
        idCheckLine.checked = true;
    }

}
function CheckDescCom() {
    var idCheckLine = document.getElementById('idCheckLine');
    var idCheckDesc = document.getElementById('idCheckDesc');
    if (idCheckDesc.checked) {
        OnchangeddlModeloCveC("Descontinuados");
        //Clear();
        idCheckLine.checked = false;
        idCheckDesc.checked = true;
        $("#articuloen").val("Descontinuados");
    } else {
        idCheckDesc.checked = true;
    }
}
$(document).ready(function () {
    //DATALIST MODELOS
    $("#datalistmodeloscom").on('input', function () {
        var val = $('#datalistmodeloscom').val();
        var dModelo = $('#datalistOptions').find('option[value="' + val + '"]').data('code');
        var ccortamodelo = $('#ccortamodelo').val();
        if (ccortamodelo == "Modelo") {
            var is = "";
        }
        else {
            var Name = $('#datalistOptions').find('option[value="' + val + '"]').data('name');
            val = Name;
        }
        $("#item").val(val);
        $('#code').val(dModelo);
        document.getElementById('cantidad').disabled = false;
        document.getElementById('comentarios').disabled = false;
        document.getElementById('addarticle').disabled = false;
    });
    $(document).on('click', '.btn_remocom', function () {
        var button_id = $(this).attr("id");
        var DateSelected = $('#rowc' + button_id + '').find('td:first').html();
        //cuando da click obtenemos el id del boton
        $('#rowc' + button_id + '').remove(); //borra la fila
        //limpia el para que vuelva a contar las filas de la tabla
        $("#adicionados").text("");
        var nFilas = $("#tbBodyCom tr").length;
        $("#adicionados").append(nFilas - 1);
    });

    $(document).on('click', '.erasetable', function () {

        $("#tbBodyCom  > tbody").empty();
    });

    $(document).on('click', '#AddPurchase', (e) => {
        e.preventDefault();
        var TableArticulos = new Array();
        var Fila = $('#tbBodyCom tr').length;
        $('#tbBodyCom tr').each(function (row, tr) {
            TableArticulos[row] = {
                "Id": Fila
                , "Linea": $(tr).find('td:eq(0)').text()
                , "Modelo": $(tr).find('td:eq(1)').text()
                , "Cantidad": $(tr).find('td:eq(2)').text()
                , "comentarios": $(tr).find('td:eq(3)').text()
            }
        });
        TableArticulos.shift();

        //llena Json 
        var addpurchase = {
            "Idstore": $('#idstore').val(),
            "Idusuario": $('#Idusuario').val(),
            "ArrayArticulos": TableArticulos,
            "WhsID": $('#WhsID').val(),
            "DEFAULTLIST": $('#DEFAULTLIST').val()
        }
        $.ajax({
            type: "POST",
            url: '/Compras/AddPurchase',
            data: "{'CapPurchase': '" + JSON.stringify(addpurchase) + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var result = JSON.parse(msg);
                var IdCompra = parseInt(result.Idcompra);
                window.location.href = "/ComprasRPTS/Download_CompraDormimundo_PDF?IDCOMPRA=" + IdCompra;
                Swal.fire({
                    icon: 'success',
                    title: 'Éxito',
                    text: 'ESPERE A QUE DESCARGUE EL PDF',
                    timer: 4000
                }).then((result) => {
                    if (result.isConfirmed) {
                        location.reload();
                        tr.hide();
                    }
                });
            },
            error: function (xhr) {
                console.log(xhr.responseText);
                alert("Error has occurred..");
            }
        });
    });
});

function AddArticles() {
    event.preventDefault();
    var table = $("#tbBodyCom");
    var id = 0;
    var modelo = $('#datalistmodeloscom').val();
    var item = $("#item").val();
    var code = $('#code').val();
    var cantidad = $('#cantidad').val();
    var comentarios = $('#comentarios').val();
    if (code == undefined || code == "") {
        MessageError("Debe seleccionar un modelo válido");
        return;
    }
    if (item == undefined || item == "") {
        MessageError("Debe agregar un modelo correcto");
        return;
    }
    var i = 1
    var i = $('#tbBodyCom tr').length;
    table.append('<tr id="rowc' + i + '"> <td>' + code + "" + "</td> <td>" + item + "</td> <td>" + cantidad + "</td> <td>" + comentarios + '</td> <td><button type="button" name="remove" id="' + i + '" class="btn btn-danger btn-sm btn_remocom"><i class="fa fa-trash" aria-hidden="true"></i>Eliminar</button></td> </tr>');
    Clear();
}
function BtnReporReimCompras() {
    let valTienda = $('#OpTiendaCompras').val().toString();
    if (valTienda == "Todos") { valTienda = -1 } else { }
    let FechaInReimCompras = $('#FechaInReimCompras').val().toString();
    let FechaFinnReimCompras = $('#FechaFinnReimCompras').val().toString();

    let Reports = {
        "IdStore": valTienda,
        "FechaIN": FechaInReimCompras,
        "FechaFin": FechaFinnReimCompras
    }

    try {

        $("#modalLoading").modal("show");

        $('#tblReimpresionCompras > thead').empty(); //Se elimina al iniciar

        $.ajax({
            type: "POST",
            url: "/Compras/GetReimpComprasAP",
            data: "{'ObjCom' : " + JSON.stringify(Reports) + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: (result) => {
                var dataset = result;
                var result1 = result;
                var columns = "";
                if (dataset != null) {

                    var spanish = {
                        "processing": "Procesando...",
                        "lengthMenu": "Mostrar _MENU_ registros",
                        "zeroRecords": "No se encontraron resultados",
                        "emptyTable": "Ningún dato disponible en esta tabla",
                        "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                        "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                        "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                        "search": "Buscar por tienda:",
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

                    var tableT = $("#tblReimpresionCompras").dataTable({
                        destroy: true,
                        language: spanish,
                        //searching: false,
                        scrollX: true,
                        iDisplayLength: 12,
                        //scrollCollapse: true,
                        fixedColumns: true,
                        select: false,
                        data: result1,
                        columns: [
                            { title: "IdInterno", data: "IdInterno" },
                            { title: "Documento SAP", data: "DocumentoSAP" },
                            { title: "Referencia SAP", data: "ReferenciaSAP" },
                            { title: "Artículo", data: "Articulo" },
                            { title: "Cantidad", data: "Cantidad" },
                            { title: "Linea SAP", data: "LineaSAP" },
                            { title: "Fecha SAP", data: "FechaSAP" },
                            { title: "Socio de negocio", data: "Socio" },
                            { title: "Usuario", data: "Usuario" },
                            { title: "Tienda", data: "Tienda" },
                            { title: "U_FOLPOS", data: "U_FOLPOS" },
                            { title: "Reimprimir", data: "btnDownloadcompra" }
                        ],
                        "columnDefs": [
                            { "type": "numeric-comma", targets: 3 }
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
                    $("#tblReimpresionCompras").dataTable().empty();
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
}
function GetReimprimeCOmpras(id) {

    $('#tblReimpresionCompras tbody').on('click', 'tr', function () {
        
        window.location.href = "/ComprasRPTS/Download_CompraDormimundo_PDF?IDCOMPRA=" + id;
        window.location = rutaReports.replace("''", "");
    });
}

