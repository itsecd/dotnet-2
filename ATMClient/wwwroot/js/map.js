let map = L.map('map').setView([53.195878, 50.100202], 13);
let mapElem = document.querySelector('#map');
let listElem = document.querySelector('.atm-list');

mapElem.style.height = window.innerHeight * 0.9 + 'px';
listElem.style.maxHeight = window.innerHeight * 0.9 + 'px';

L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
    attribution: '<a href="https://github.com/vladahaustova">Vlada Khaustova</a>',
    maxZoom: 18,
    id: 'mapbox/streets-v11',
    tileSize: 512,
    zoomOffset: -1,
    accessToken: 'pk.eyJ1Ijoidm9sdW1lY29yZSIsImEiOiJjbDJxN3cwa3QycjRwM2NwOTkwdmUzZGRwIn0.aGbKOtZKHqTyQ2SQKRm6_g'
}).addTo(map);

// var marker = L.marker([53.195878, 50.100202]).addTo(map);

// marker.bindPopup("<b>Hello world!</b><br>I am a popup.").openPopup();

let selectedATMId = "";
let markers = {};

fetch(url + 'api/ATM', {})
	.then(response => response.json())
	.then(result => { 
		console.log(result);
		for (let atm of result) {
			let marker = L.marker([atm.geometry.coordinates[1], atm.geometry.coordinates[0]]).addTo(map)
				.bindPopup("<b>" + atm.properties.operator + "</b><br>Баланс: " + atm.properties.balance + "<br><input class='input-new-balance' type='text'><span class=\"focus-border\"></span><br><button class='change-balance-button' onclick='changeBalance()'>Поменять баланс</button>")
				.addEventListener("click", () => {
					selectedATMId = atm.properties.id;
				});
			markers[atm.properties.id] = marker;

			let newItem = document.createElement('div');
			newItem.innerHTML = `
			<div class="atm-list__item">
     			<b class="atm-list__item_title">${atm.properties.operator}</b>
     			<p class="atm-list__item_balance">Баланс: ${atm.properties.balance} $</p>
    		</div>
			`;
			newItem.addEventListener("click", () => {
				markers[atm.properties.id].openPopup();
				map.setView([atm.geometry.coordinates[1], atm.geometry.coordinates[0]], 13);
				selectedATMId = atm.properties.id;
			})

			listElem.appendChild(newItem);
		}
	})
	
async function changeBalance() {
	try {
		const response = await fetch(url + 'api/ATM/' + selectedATMId, {
		method: 'PUT', // или 'PUT'
		body: document.querySelector('.input-new-balance').value, // данные могут быть 'строкой' или {объектом}!
		headers: {
			'Content-Type': 'application/json'
		}
	});
	const json = await response.json();
	console.log('Успех:', json);
	markers[json.properties.id].bindPopup("<b>" + json.properties.operator + "</b><br>Баланс: " + json.properties.balance + "<br><input class='input-new-balance' type='text'><span class=\"focus-border\"></span><br><button class='change-balance-button' onclick='changeBalance()'>Поменять баланс</button>");

	let editedATMIndex = 0;
	for (let key in markers) {
		editedATMIndex++;
		if (key === json.properties.id)
			break;
	}

	listElem.childNodes[editedATMIndex].innerHTML = `
		<div class="atm-list__item">
     		<b class="atm-list__item_title">${json.properties.operator}</b>
     		<p class="atm-list__item_balance">Баланс: ${json.properties.balance} $</p>
    	</div>
		`;

	} catch (error) {
		console.error('Ошибка:', error);
	}
}