window.addEventListener('resize', () => {
	mapElem.style.height = window.innerHeight * 0.9 + 'px';
	listElem.style.maxHeight = window.innerHeight * 0.9 + 'px';
})