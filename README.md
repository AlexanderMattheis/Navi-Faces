# Navi Faces
A very old project integrated in my unfinished Navi-Game. The architecture was too complicated
in comparison to the MVC-pattern similar architectures I create now.
Also, the description is currently only available in German. It is about a new language which allows you
to design multilingual, resolution independent menus in computer games very fast/time efficient due to a hard separation of logic
and design e.g. a fully working main menu with four buttons written in the Faces-language:

```ruby
import(Windows.Main);

Define:
color = #color=Button;
highlight = #color=ButtonHighlight;
sound = #sound=Widget;
text = #post#db;
textColor = #color=TextBody;

Create:
btn{New, Controls, Credits, Exit}(color, highlight, sound, text, textColor);

Link:
btnNew[N]->MapLoading;
btn{Controls, Credits}->#post;
btnExit[E]->#exit;

Add:
(btn{New, Controls, Credits}, 50.0, 39.2[+10.0], 36.5, 7.29);
(btnExit, y = 83.3);
```

## Developer-Funktionen
Funktionen, denen der Entwickler eine bestimmte Funktion zugeteilt hat. Diesen Funktionen sind unterschiedliche Rechte zu Teil. Daher müssen sie unterschiedlich gehandhabt werden. Beispielsweise erlaubt die ``Add``-Funktion die Übergabe von Widgets ans Surface, während in der ``Define``-Funktion lediglich Variablen definiert werden können.

### 1. Define 
Definition von Variablen, die für die Objekte benötigt werden.

#### Beispiele:
```ruby
Define:
image = Gui.MenuButton;
shinyImage = Gui.MenuButtonHighlight;
pivot = (50.0, 50.0);
```

Die Deklaration von Variablen sollte nach Konvention in alphabetischer Reihenfolge bzgl. des Variablentyps erfolgen. 
Man könnte die Variablen mit den primitiven Variablentypen gängiger Programmiersprachen vergleichen.

### 2. Create: 
Zum Erstellen von Objekten/Widgets.

#### Beispiele:
```ruby
Create:
btn{New, Controls, Credits, Exit}(color, highlight, sound, text, textColor);
lblChooseAMap(text, textColor);
```

wobei: <br>
``lbl``: Datentyp <br>
``ChooseAMap``: Objektname

Dient der Erstellung sichtbarer Oberflächenobjekte (``img..., btn...``). Variablen können, aber müssen nicht an die Objekte übergeben werden. Variablen (``color, highlight, sound, ...``) werden in alphabetischer Reihenfolge übergeben.

#### Objekt-Arrays
```ruby
btn{New, Credits, ...}([parameter])
```

### 3. Link
Zum Verlinken von Objekten.

#### Beispiele:
```ruby
Link:
btnNew[N]->MapLoading;
btn{Controls, Credits}->#post;
btnExit[E]->#exit;
```

Zuvor erstellte Objekte werden auf andere Surfaces/Menüs verwiesen und Funktionen zugeteilt. Objekte werden mit dem ``->``- Operator verlinkt. Das heißt, die darauffolgende Funktion oder das jeweilige Surface/Menü wird aufgerufen.

#### Kommandos
``#post``: nimmt den Objektnamen <br>
``#big``: der String an den ``#big`` angehängt wird, wird in Großbuchstaben geschrieben <br>
``#exit``: verlässt die Anwendung

#### Stellvertreter/Shortcuts
```
btnNew[N]->MapLoading;
```
bedeutet die Taste N ist ein Stellvertreter/Shortcut beim ``MapLoading``.
Heißt, über die Taste gelangt man in das ``MapLoading``-Menü.

### 4. Add 
Zum Hinzufügen der Objekte zum Surface.

```ruby
Add:
(btn{New, Controls, Credits}, 50.0, 39.2[+10.0], 36.5, 7.29);
(btnExit, y = 83.3);
```

## Variablen
Variablen wird der Wert auf der rechten Seite (also nach dem ``=``) zugewiesen. Variablen werden Objekten übergeben.
Variablen können aufgezählt werden
```ruby
image_1 = Logos.navi_logo_white;
image_2 = Gui.MenuButton;
...
```

**Hinweis:** Eigene Variablen können nicht deklariert werden! Alle verfügbaren Variablen sind unten drunter aufgelistet.

### Buttons:
- color
- font
- image
- highlight
- pivot
- shinyImage
- text
- textAligment
- textColor
- textScale
- textPivot

### Images:
- color
- image
- pivot

### Labels:
- color
- image
- font
- pivot
- text
- textAligment
- textColor
- textScale
- textPivot

### System:
- alignment
- dimensions

## Parameterübergabe
Die Parameterübergabe findet über die Parameterklammern ``(`` und ``)`` statt. 
Es wird jeweils zur nächsthöheren Systemebene übergeben.

### Ebenen
1. Screen <br>                                                                                                                                                   
2. Surface <br>								                  
3. Widget (``img..., btn...``) <br>

also: <br>
Variablen -> Widget -> Surface -> Screen

#### Beispiele:
```ruby
import(Windows.Main);
dimensions = (0.0, 0.0, 100.0, 100.0);
alignment = center;
```

Somit wird dem  das Hauptfenster ``Main`` übergeben und die zugehörigen Variablen gesetzt. 
Wobei diese in genau den hier zwei gezeigten Fall weggelassen werden können, 
da es sich auf der rechten Seite um Default-Werte handelt.

```ruby
Create:
btn{New, Controls, Credits, Exit}(color, highlight, sound, text, textColor)

Add:
(btnNew, 50.0, 39.3, 36.5, 7.29);
```

Somit werden Variablen ``color, highlight, sound, text, textColor`` an Objekte ``btn`` übergeben
und der eine ``btn`` mit den gegebenen Parametern ``50.0, 39.3, 36.5, 7.29`` ans Surface übergeben.
