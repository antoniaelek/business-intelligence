Kreirana je baza `pi` i dimenzijska tablica `dDate`.
Tablica je napunjena programski, na nacin da se u .NET konzolnoj aplikaciji najprije izgenerira lista svih dana (klasa `Day`) koji pripadaju rasponu godina od 2000 do 2018, a zatim se ta lista unese u bazu.

Za dohvat praznika u navedenom vremenskom razdoblju kori�tena je komponenta [Dynamic Holiday Date Calculator](https://www.codeproject.com/Articles/11666/Dynamic-Holiday-Date-Calculator).
Za mapiranje objekata na relacijsku tablicu te za punjenje iste kori�ten je ORM [Dapper](https://www.nuget.org/packages/Dapper.Contrib/).