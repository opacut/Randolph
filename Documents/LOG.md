# Log

## 27. 2. 2018

### Poznámky, pozrieť sa na:

* [ProCamera2D](http://www.procamera2d.com/)
* [Tutoriály pixelart na Pintereste](https://cz.pinterest.com/search/pins/?q=pixel%20art%20tutorial&rs=typed&term_meta[]=pixel%7Ctyped&term_meta[]=art%7Ctyped&term_meta[]=tutorial%7Ctyped)
* [Hexels](https://www.marmoset.co/hexels/)

### V gite 

* Pri práci vytvoríš na task vlastnú branch, keď task dokončíš tak ju mergeneš
    * Tool na merge:
	    * [Sourcetree](https://www.sourcetreeapp.com/)
	    * [Github for Unity](https://unity.github.com/)
	    * [Smart Git](https://www.syntevo.com/smartgit/)
		* [GitKraken](https://www.gitkraken.com/github-student-developer-pack) ([+ LFS](https://support.gitkraken.com/git-workflows-and-extensions/Intro-and-requirements))

### Character 

* Ideálne **32×16**. Jeden tile je teda **16×16**, objekt môže byť na **½ tile**, **¼ tile**…

### Mechaniky pre level design (idea pool)

* Schody
* Hub zaznamenávajúci progress
* Mapa
* Chrám
* Liana nad priepasťou
* Laso

### Tasks

* **Matúš** – tileset nakresliť
* **Tomáš** – Menu, level načítanie
* **Oliver** – puzzle design, story
* **Ondřej** – design hub (the ship)

---

## 17. 4. 2018

### BUGS

* Velocity predmetov je zachovaná pri restarte
* V Runner lvl je možné dostať sa do stavu odkiaľ sa hra nedá prejsť
* Na moving platform sa Randolph trasie

### TODO

* Načítání (nepoznačil som si bližšie, čo sme tým mysleli)
* Odstrániť oranžové bloky
* Tutorial lvl
* Animácia crows
* Ubrať zhora 5 tiles
* Nahoru dát zeď
* Vnútro lode - hub
* Pickup predmetov - myšou
* Namiesto spikes v Runner lvl použiť pomaly sa otvárajúcu bránu
* Hook - naučiť hráča ako ho používať
* Hook - (optional) prychitiť na rebrík
* Hook - (optional) použiť na stiahnutie kameňa v Runner lvl
* Optimalizovať radius predmetov
* Viac levelov!
* (optional) Suicide button

### Notes
* Jeden level bude rozsahom (počet obrazoviek) veľký asi ako Horizontal Slice
* Predbežná schéma kôl hry:
	1. Skaly
	2. Džungľa
	3. Chrám
	4. Chrám
* Návrh na vertikálny lvl, kde napr. uteká smerom hore pred záhubou, viz [Fossil Echo](https://youtu.be/jEOV6CNi3So?t=2m14s)