'use strict';

const switcher = document.querySelector('#btn');
// const switcher = document.getElementById('btn');
switcher.addEventListener('click', () => {
    document.body.classList.toggle('light-theme');
    document.body.classList.toggle('dark-theme');

    const className = document.body.className;
    this.textContext = className == "light-theme" ? "Dark" : "Light";
    console.log(`Current class name: ${className}.`)
})