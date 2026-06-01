Examen — Variantă 1

Instrucțiuni generale
- Lucrări în repo-ul TaskFlow (calea rădăcină a proiectului).
- Rulați testele
- Livrabile: cod corectat/implementat, migrații generate (dacă e cazul), teste trecute

#### 1) Subiect 1 — Fixarea testului care pică (obligatoriu) (10p)

##### Descriere:
Rulați testele și veți observa unul (sau mai multe) test(e) care eșuează în TaskFlow.BLL.UnitTests. Pare ca cineva a stricat sau a sters o bucata de cod esentiala. Testul se asteapta la ceva, dar codul nu face.

##### Ce trebuie să faceți:
  - Implementați functionalitatea conform comportamentului așteptat 
  - Asigurați-vă că unit-testul trece fara modificari pe test in sine

#### 2) Subiect 2 — Centralizare handling excepții în aplicație (refactor) (10p)
##### Descriere:
În controlere apare handling repetitiv pentru erori/exceptii (de exemplu blocuri try/catch care transformă excepții în 4xx/5xx). Această repetare ar trebui eliminată: excepțiile care sunt *handled* trebuie tratate într-un singur loc comun, nu în fiecare acțiune din controller.
##### Ce trebuie să faceți:
  - Scoateți handling-urile repetitive din metodele controllerelor (îndepărtați try/catch duplicat).
  - Updatati o componentă centrală care convertește excepțiile tratate în răspunsuri HTTP 
  - Loghează detaliile erorii și returnează un body JSON minim cu mesajul.
  - Păstrați experiența API-ului la fel din perspectiva clientului (codurile de stare așteptate) dar eliminați duplicarea din controllere.

#### 3) Subiect 3 — Adăugare entitate Goal (20p)
##### Descriere scurtă:
Adăugați o entitate **Goal** legată de Project (FK ProjectId). Aceasta servește pentru scopuri de planificare a proiectelor. Aceasta va avea structura de mai jos
  - GoalEntity:
    - int Id
    - int ProjectId (FK către ProjectEntity)
    - string Title (required, max length 250)
    - DateTime TargetDate (business rule: trebuie să fie nu fie in trecut)
    - bool IsCompleted

##### Ce trebuie sa faceti:
Trebuie sa implementati de la cap la coada functionalitatea cu:
  - endpoint GET ALL, CREATE si DELETE, cu status code urile corecte
  - DELETE trebuie restrictionat la Admin only
  - GET ALL, CREATE vor fi autorizate la orice User
  - Entitate, migrari, Repository, Service Layer, Validari Business, Dto, Controllere


---
