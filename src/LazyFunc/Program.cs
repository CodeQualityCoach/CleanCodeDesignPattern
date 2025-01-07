using LazyFunc;
using StructureMap;

/********************************************************
 * Aufgabe:
 * 
 * Schau dir den Code und die Factories an. 
 * 
 * Wann werden neue Instanzen erzeugt?
 * Wer erzeugt die Instanzen, der DI Container oder die Factory?
 * 
********************************************************/

Console.WriteLine("Hello, LazyFunc!");

var l = new Lazy<IOut>(() => new ConsoleOut());
var f = new Func<IOut>(() => new ConsoleOut());

l.Value.Log("Good morning coder");
f().Log("And have a nice day");

// lets do some DI
var container = new Container(_ =>
{
    // what happens when I make this a singleton. How many instances are created?
    //_.For<IOut>().Use<ConsoleOut>().Singleton(); 
    _.For<IOut>().Use<ConsoleOut>();

    _.For<IOutFactory>().Use<OutFactory>();
    _.For<IOutFactory>().Use<OutFuncFactory>();

    // what happens when I make this a singleton. How many instances are created (of IOut)?
    //_.For<IOutFactory>().Use<OutLazyFactory>().Singleton();
    _.For<IOutFactory>().Use<OutLazyFactory>();
}
);

var c1 = container.GetInstance<IOut>();
c1.Log("I am just an instance");

var c2 = container.GetInstance<Lazy<IOut>>();
c2.Value.Log("I am lazy");
c2.Value.Log("I am lazy but can be used twice");

var c3 = container.GetInstance<Lazy<IOut>>();
c3.Value.Log("I am another lazy");
c3.Value.Log("I am lazy but can be used twice");

var c4 = container.GetInstance<Func<IOut>>();
c4().Log("I am a func, btw");
c4().Log("I am a func and I log again");

var c5 = container.GetAllInstances<IOutFactory>();
foreach (var outFactory in c5)
{
    outFactory.Create().Log($"I am in an iteration and I am {outFactory.GetType().Name}");
    outFactory.Create().Log($"I am in an iteration and I am {outFactory.GetType().Name} again");
}


var c6 = container.GetAllInstances<IOutFactory>();
foreach (var outFactory in c6)
{
    outFactory.Create().Log($"I am in the second iteration and I am {outFactory.GetType().Name}");
    outFactory.Create().Log($"I am in the second iteration and I am {outFactory.GetType().Name} again");
}

// ein Bonus: ContainerFactory
var c9 = container.GetInstance<ContainerFactory>(); // Wait, ContainerFactory is not registered!
c9.Create().Log("I am here from the container factory");

Console.ReadKey();