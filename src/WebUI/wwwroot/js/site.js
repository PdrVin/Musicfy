// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    getDataTable('#table-artist');
    getDataTable('#table-album');
    getDataTable('#table-music');
    getDataTable('#table-playlist');
})

function getDataTable(id) {
    $(id).DataTable({
        "ordering": true,
        "paging": true,
        "searching": true,
        "columnDefs": [
            { "className": "text-center", "targets": "_all" }
        ],
        "oLanguage": {
            "sEmptyTable": "Nenhum registro encontrado na tabela",
            "sInfo": "Mostrar _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrar 0 até 0 de 0 Registros",
            "sInfoFiltered": "(Filtrar de _MAX_ total registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Mostrar _MENU_ registros por pagina",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Proximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Ultimo"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
    });
}

$(".close-alert").click(function () {
    $(".alert").hide('hide');
});

function addDynamicRow(containerId, fieldConfigs) {
    const container = document.getElementById(containerId);
    const index = container.children.length;

    const div = document.createElement('div');
    div.className = "input-group mb-3 bg-body";

    const span = document.createElement('span');
    span.className = "input-group-text bg-body text-white";
    span.textContent = index + 1;
    div.appendChild(span);

    for (const config of fieldConfigs) {
        const input = document.createElement('input');
        input.type = config.type || "text";
        input.id = `${index}__${config.name}`;
        input.name = `[${index}].${config.name}`;
        input.placeholder = config.placeholder;
        input.className = config.className || "form-control bg-body text-white";

        input.setAttribute("data-val", "true");
        input.setAttribute("data-val-required", `O campo ${config.name} é obrigatório.`);

        div.appendChild(input);

        const validationSpan = document.createElement("span");
        validationSpan.className = "text-danger field-validation-valid";
        validationSpan.setAttribute("data-valmsg-for", `[${index}].${config.name}`);
        validationSpan.setAttribute("data-valmsg-replace", "true");

        div.appendChild(validationSpan);
    }

    container.appendChild(div);
}

function removeDynamicRow(containerId) {
    const container = document.getElementById(containerId);
    if (container.children.length > 1) {
        container.removeChild(container.lastElementChild);
    }
}
