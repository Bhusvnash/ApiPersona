//api rest
const url = "http://localhost:5099/Persona"
async function getAll() {
  try {
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

async function update(id, persona){
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

