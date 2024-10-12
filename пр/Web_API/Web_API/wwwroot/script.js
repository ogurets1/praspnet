async function getHotels() {
    // отправляет запрос и получаем ответ
    const response = await fetch("/api/hotels", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    // если запрос прошел нормально
    if (response.ok === true) {
        // получаем данные
        const hotels = await response.json();
        const rows = document.querySelector("tbody");
        // добавляем полученные элементы в таблицу
        hotels.forEach(hotel => rows.append(row(hotel)));
    }
}
// Получение одного пользователя
async function getHotel(id) {
    const response = await fetch(`/api/hotels/${id}`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        const hotel = await response.json();
        document.getElementById("HotelId").value = hotel.id;
        document.getElementById("HotelName").value = hotel.name;
        document.getElementById("HotelCount").value = hotel.countOfStars;
        document.getElementById("HotelCode").value = hotel.countryCode;
    }
    else {
        // если произошла ошибка, получаем сообщение об ошибке
        const error = await response.json();
        console.log(error.message); // и выводим его на консоль
    }
}
// Добавление пользователя
async function createHotel(HotelName, HotelCount, HotelCode) {

    const response = await fetch("api/hotels", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            name: HotelName,
            countOfStars: parseInt(HotelCount, 10),
            countryCode: HotelCode
        })
    });
    if (response.ok === true) {
        const hotels = await response.json();
        document.querySelector("tbody").append(row(hotels));
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}
// Изменение пользователя
async function editHotel(HotelId, HotelName, HotelCount, HotelCode) {
    const response = await fetch("api/hotels", {
        method: "PUT",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            id: HotelId,
            name: HotelName,
            countOfStars: parseInt(HotelCount, 10),
            countryCode: HotelCode
        })
    });
    if (response.ok === true) {
        const hotel = await response.json();
        document.querySelector(`tr[data-rowid='${hotel.id}']`).replaceWith(row(hotel));
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}
// Удаление пользователя
async function deleteHotel(id) {
    const response = await fetch(`/api/hotels/${id}`, {
        method: "DELETE",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        const hotel = await response.json();
        document.querySelector(`tr[data-rowid='${hotel.id}']`).remove();
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}

// сброс данных формы после отправки
function reset() {
    document.getElementById("HotelId").value =
        document.getElementById("HotelName").value =
        document.getElementById("HotelCode").value =
        document.getElementById("HotelCount").value = "";
}
// создание строки для таблицы
function row(hotel) {

    const tr = document.createElement("tr");
    tr.setAttribute("data-rowid", hotel.id);

    const IdTd = document.createElement("td");
    IdTd.append(hotel.id);
    tr.append(IdTd);

    const nameTd = document.createElement("td");
    nameTd.append(hotel.name);
    tr.append(nameTd);

    const countTd = document.createElement("td");
    countTd.append(hotel.countOfStars);
    tr.append(countTd);

    const codeTd = document.createElement("td");
    codeTd.append(hotel.countryCode);
    tr.append(codeTd);

    const linksTd = document.createElement("td");

    const editLink = document.createElement("a");
    editLink.setAttribute("data-id", hotel.id);
    editLink.setAttribute("style", "cursor:pointer;padding:15px;");
    editLink.append("Изменить");
    editLink.addEventListener("click", async () => await getHotel(hotel.id));
    linksTd.append(editLink);

    const removeLink = document.createElement("a");
    removeLink.setAttribute("data-id", hotel.id);
    removeLink.setAttribute("style", "cursor:pointer;padding:15px;");
    removeLink.append("Удалить");
    removeLink.addEventListener("click", async () => await deleteHotel(hotel.id));

    linksTd.append(removeLink);
    tr.appendChild(linksTd);

    return tr;
}
// сброс значений формы
document.getElementById("resetBtn").addEventListener("click", () => reset());

// отправка формы
document.getElementById("saveBtn").addEventListener("click", async () => {

    const id = document.getElementById("HotelId").value;
    const name = document.getElementById("HotelName").value;
    const count = document.getElementById("HotelCount").value;
    const code = document.getElementById("HotelCode").value;
    if (id === "")
        await createHotel(name, count, code);
    else
        await editHotel(id, name, count, code);
    reset();
});

getHotels();