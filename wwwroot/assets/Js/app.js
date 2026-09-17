// ==========================================
// APP - Orquestador Principal
// Responsabilidad: Gestionar estado, formularios, validación e interactuar con la API
// ==========================================

// --- ESTADO GLOBAL ---
let listaPersonas = [];

// --- REFERENCIAS AL DOM ---
const btnNew = document.querySelector("#btnNew");
const inputFilter = document.querySelector("#inputFilter");
const tabla = document.querySelector("#tabla");

// Objeto de Control del Modal (Bootstrap)
const modal = {
  instance: null,
  element: document.querySelector('#personModal'),
  titulo: document.querySelector('#modalTitle'),
  body: document.querySelector("#modalBody"),
  footer: document.querySelector("#modalFooter"),

  init: () => {
    if (!modal.instance && modal.element) {
      modal.instance = new bootstrap.Modal(modal.element);
    }
  },

  clear: () => {
    modal.titulo.textContent = "";
    modal.body.innerHTML = "";
    modal.footer.innerHTML = `
      <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
    `;
  },

  show: () => {
    modal.init();
    modal.instance.show();
  },

  hide: () => {
    if (modal.instance) modal.instance.hide();
  }
};

// --- RENDERIZADO Y CARGA DE DATOS ---

/**
 * Carga la lista inicial desde la API y renderiza la tabla
 */
async function cargarPersonas() {
  try {
    listaPersonas = await apiGetAll();
    renderizarTabla(listaPersonas);
  } catch (error) {
    tabla.innerHTML = "";
    tabla.appendChild(crearFilaVaciaTabla("Error al cargar los datos desde la API"));
  }
}

/**
 * Renderiza la tabla según la lista de personas proporcionada
 */
function renderizarTabla(personas) {
  tabla.innerHTML = "";
  if (!personas || personas.length === 0) {
    tabla.appendChild(crearFilaVaciaTabla("No hay personas registradas"));
    return;
  }
  personas.forEach(persona => {
    const fila = crearFilaTabla(persona);
    tabla.appendChild(fila);
  });
}

/**
 * Filtra la lista en tiempo real por ID o Nombre
 */
function filtrarPersonas(criterio) {
  const termino = criterio.trim().toLowerCase();
  if (!termino) {
    renderizarTabla(listaPersonas);
    return;
  }
  const filtradas = listaPersonas.filter(p => 
    p.id.toString().includes(termino) || 
    p.nombre.toLowerCase().includes(termino)
  );
  renderizarTabla(filtradas);
}

// --- GESTIÓN DE FORMULARIOS Y MODALES ---

/**
 * Abre el modal para agregar una nueva persona
 */
function modalNuevaPersona() {
  modal.clear();
  modal.titulo.textContent = "Agregar Nueva Persona";

  const form = crearFormularioPersona();
  modal.body.appendChild(form);

  const btnGuardar = crearButton({
    text: "Guardar",
    accion: "guardarContacto",
    color: "btn-primary"
  });

  modal.footer.appendChild(btnGuardar);
  modal.show();
}

/**
 * Abre el modal cargando los datos de una persona para editar
 */
async function modalEditarPersona(id) {
  try {
    modal.clear();
    modal.titulo.textContent = `Editar Persona #${id}`;

    const persona = await apiGetById(id);
    const form = crearFormularioPersona(persona);
    modal.body.appendChild(form);

    const btnActualizar = crearButton({
      text: "Actualizar",
      accion: "actualizarContacto",
      color: "btn-success",
      dataId: id
    });

    modal.footer.appendChild(btnActualizar);
    modal.show();
  } catch (error) {
    alert("No se pudo cargar la información de la persona seleccionada.");
  }
}

// --- VALIDACIÓN Y OPERACIONES DE NEGOCIO ---

/**
 * Valida los datos introducidos en el formulario
 */
function obtenerYValidarDatosFormulario() {
  const nombreInput = document.querySelector("#inputNombre");
  const telefonoInput = document.querySelector("#inputTelefono");

  const nombre = nombreInput ? nombreInput.value.trim() : "";
  const telefono = telefonoInput ? telefonoInput.value.trim() : "";

  if (!nombre) {
    alert("El campo Nombre es obligatorio.");
    if (nombreInput) nombreInput.focus();
    return null;
  }

  if (!telefono) {
    alert("El campo Teléfono es obligatorio.");
    if (telefonoInput) telefonoInput.focus();
    return null;
  }

  return { nombre, telefono };
}

/**
 * Procesa el guardado (creación) de una persona
 */
async function procesarGuardarPersona() {
  const datos = obtenerYValidarDatosFormulario();
  if (!datos) return;

  try {
    await apiCreate(datos);
    modal.hide();
    await cargarPersonas();
  } catch (error) {
    alert("Ocurrió un error al guardar los datos.");
  }
}

/**
 * Procesa la actualización de una persona existente
 */
async function procesarActualizarPersona(id) {
  const datos = obtenerYValidarDatosFormulario();
  if (!datos) return;

  datos.id = parseInt(id, 10);

  try {
    await apiUpdate(id, datos);
    modal.hide();
    await cargarPersonas();
  } catch (error) {
    alert("Ocurrió un error al actualizar la persona.");
  }
}

/**
 * Procesa la eliminación de una persona
 */
async function procesarEliminarPersona(id) {
  const confirmacion = confirm(`¿Estás seguro de que deseas eliminar la persona #${id}?`);
  if (!confirmacion) return;

  try {
    await apiDeleteById(id);
    await cargarPersonas();
  } catch (error) {
    alert("No se pudo eliminar el registro seleccionado.");
  }
}
