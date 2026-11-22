// Muestra el total estimado mientras el usuario llena el formulario de reserva
const selectDestino = document.getElementById("DestinoId");
const inputPersonas = document.getElementById("Personas");
const inputFecha = document.getElementById("Fecha");
const estimado = document.getElementById("estimado");

function calcularEstimado() {
    const opcion = selectDestino.options[selectDestino.selectedIndex];
    const precio = parseInt(opcion.dataset.precio);
    const personas = parseInt(inputPersonas.value);

    if (isNaN(precio) || isNaN(personas) || personas < 1 || personas > 10) {
        estimado.textContent = "Elige un destino y entre 1 y 10 personas para ver el total estimado.";
        return;
    }

    estimado.textContent = "Total estimado: $" + (precio * personas).toLocaleString("es-CO");
}

// Solo se ejecuta en las páginas que tienen el formulario de reserva
if (selectDestino && inputPersonas && estimado) {
    selectDestino.addEventListener("change", calcularEstimado);
    inputPersonas.addEventListener("input", calcularEstimado);

    // En una reserva nueva no deja elegir fechas pasadas
    if (inputFecha && !inputFecha.value) {
        inputFecha.min = new Date().toLocaleDateString("en-CA");
    }

    calcularEstimado();
}
