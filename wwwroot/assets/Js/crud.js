// ==========================================
// CRUD - Comunicación con la API REST (/Persona)
// Responsabilidad: Peticiones HTTP con rutas relativas
// ==========================================

const API_URL = "/Persona";

/**
 * Obtiene la lista completa de personas
 * @returns {Promise<Array>}
 */
async function apiGetAll() {
  try {
    const response = await fetch(API_URL);
    if (!response.ok) throw new Error(`Error en la petición: ${response.status}`);
    return await response.json();
  } catch (error) {
    console.error("Error al obtener personas:", error);
    throw error;
  }
}

/**
 * Obtiene una persona específica por su ID
 * @param {number|string} id 
 * @returns {Promise<Object>}
 */
async function apiGetById(id) {
  try {
    const response = await fetch(`${API_URL}/${id}`);
    if (!response.ok) throw new Error(`Error al obtener ID ${id}: ${response.status}`);
    return await response.json();
  } catch (error) {
    console.error(`Error al obtener persona ${id}:`, error);
    throw error;
  }
}

/**
 * Crea una nueva persona en la base de datos
 * @param {Object} persona { nombre, telefono }
 * @returns {Promise<Object>}
 */
async function apiCreate(persona) {
  try {
    const response = await fetch(API_URL, {
      method: "POST",
      headers: { 
        "Content-Type": "application/json" 
      },
      body: JSON.stringify(persona)
    });
    if (!response.ok) throw new Error(`Error al crear persona: ${response.status}`);
    return await response.json();
  } catch (error) {
    console.error("Error al crear persona:", error);
    throw error;
  }
}

/**
 * Actualiza los datos de una persona existente
 * @param {number|string} id 
 * @param {Object} persona { id, nombre, telefono }
 * @returns {Promise<boolean>}
 */
async function apiUpdate(id, persona) {
  try {
    const response = await fetch(`${API_URL}/${id}`, {
      method: "PUT",
      headers: { 
        "Content-Type": "application/json" 
      },
      body: JSON.stringify(persona)
    });
    if (!response.ok) throw new Error(`Error al actualizar persona ${id}: ${response.status}`);
    return true;
  } catch (error) {
    console.error(`Error al actualizar persona ${id}:`, error);
    throw error;
  }
}

/**
 * Elimina una persona por su ID
 * @param {number|string} id 
 * @returns {Promise<boolean>}
 */
async function apiDeleteById(id) {
  try {
    const response = await fetch(`${API_URL}/${id}`, {
      method: "DELETE"
    });
    if (!response.ok) throw new Error(`Error al eliminar persona ${id}: ${response.status}`);
    return true;
  } catch (error) {
    console.error(`Error al eliminar persona ${id}:`, error);
    throw error;
  }
}
