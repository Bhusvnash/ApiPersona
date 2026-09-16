//api rest
const url = "http://localhost:5099/Persona"
//arreglo tem para test
let JsonData = `
[
    {
        "id": 1,
        "nombre": "Juan Perez",
        "telefono": "3001234567"
    },
    {
        "id": 2,
        "nombre": "Maria Gomez",
        "telefono": "3012345678"
    },
    {
        "id": 3,
        "nombre": "Carlos Ramirez",
        "telefono": "3103456789"
    },
    {
        "id": 4,
        "nombre": "Laura Martinez",
        "telefono": "3154567890"
    },
    {
        "id": 5,
        "nombre": "Andres Rodriguez",
        "telefono": "3205678901"
    },
    {
        "id": 6,
        "nombre": "Pedro Lopez",
        "telefono": "3001112233"
    },
    {
        "id": 7,
        "nombre": "John Doe",
        "telefono": "123456789"
    },
    {
        "id": 8,
        "nombre": "John Doe",
        "telefono": "123456789"
    }
]
`
const sendData = () => {
  return JSON.parse(JsonData)
} 
async function getAll() {
  try {
    //liena temporar: 
    return sendData();
    const response = await fetch(url)
    const data = await response.json()
    return data
  } catch (error) {
    console.log(error)
  }
}

async function getById(id) {
  try {
    const response = await fetch(`${url}/${id}`)
    const data = await response.json()
    return data
  } catch (error) {
    console.log(error)
  }
}
async function create(persona) {
  try {
    const response = await fetch(url, {
      "method": "Delete",
      "headers": { "Content - Type": "application/json" },
      "body": JSON.stringify(persona)
    })
    const data = await response.json()
    return data
  } catch (error) {
    console.log(error)
  }
}
//PUT /persona/{id}
async function update(id, persona) {
  try {
    const response = await fetch(`${url}/${id}`, {
      "method": "PUT",
      "headers": { "Content - Type": "application/json" },
      "body": JSON.stringify(persona)
    })
    const data = await response.json()
    return data
  } catch (error) {
    console.log(error)
  }
}
