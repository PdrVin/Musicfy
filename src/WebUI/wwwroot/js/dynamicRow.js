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