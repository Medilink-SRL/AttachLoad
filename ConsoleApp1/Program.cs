// Libreria File
// Iniziare una Lista List<FileEntry>
// Cercare online IEnumerable
// Ciclare ogni file contenuto nella cartella e mostrare in console il nome e la dimensione
// Attenzionare e utilizzare la direttiva using e vedere online il concetto di GarbageCollector
// creare una classe .cs FileEntry:

using ConsoleApp1;
using System.Configuration;


if (!SQLComponent.TestConnection())
{
    Console.WriteLine("Connection String Errata");
    return;
}


DirectoryInfo dir = new DirectoryInfo("Attach");

var files = dir.GetFiles();

var fileList = new List<FileEntry>();

FileEntry fe;
foreach (var file in files) {
    fe = new FileEntry()
    {
        Nome = file.Name,
        Dimensione = (int)file.Length
    };

    fileList.Add(fe);
}

foreach (var el in fileList)
{
    Console.WriteLine($"{el.Nome} | {el.Dimensione} B");
}

FileEntry user = new FileEntry() { Nome = "Sara", Dimensione = 200 };

var test = SQLComponent.Insert(user);

Console.WriteLine($"Risultato operazione: {test}");



var firstUser = SQLComponent.Get(1).Nome;

Console.WriteLine($"Utente: {firstUser}");

var allUsers = SQLComponent.GetAll();

Console.WriteLine("Utenti:");
foreach(var u in allUsers)
{
    Console.WriteLine(u.Nome);
}