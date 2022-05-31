const url = 'https://localhost:44372/'

var map = L.map('map').setView([53.195878, 50.100202], 13);

L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
    attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
    maxZoom: 18,
    id: 'mapbox/streets-v11',
    tileSize: 512,
    zoomOffset: -1,
    accessToken: 'pk.eyJ1Ijoidm9sdW1lY29yZSIsImEiOiJjbDJxN3cwa3QycjRwM2NwOTkwdmUzZGRwIn0.aGbKOtZKHqTyQ2SQKRm6_g'
}).addTo(map);	

// var marker = L.marker([53.195878, 50.100202]).addTo(map);

// marker.bindPopup("<b>Hello world!</b><br>I am a popup.").openPopup();

let ATMs = [];

fetch(url + 'api/ATM', {})
	.then(response => response.json())
	.then(result => { 
		console.log(result);
		for (let atm of result) {
			ATMs.push(atm);
			console.log(atm);
			L.marker([atm.coords.x, atm.coords.y]).addTo(map)
				.bindPopup("<b>" + atm.bankName + "</b><br>Balance: " + atm.balance);
		}
	})
	
async function changeBalance() {
	try {
		const response = await fetch(url + 'api/ATM/gjsdp10288ca8g', {
		method: 'PUT', // или 'PUT'
		body: 44444, // данные могут быть 'строкой' или {объектом}!
		headers: {
			'Content-Type': 'application/json'
		}
	});
	const json = await response.json();
	console.log('Успех:', JSON.stringify(json));
	} catch (error) {
		console.error('Ошибка:', error);
	}
}