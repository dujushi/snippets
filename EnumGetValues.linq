<Query Kind="Program">
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Linq</Namespace>
</Query>

void Main()
{
	var query1 = from CategoryEnum c in Enum.GetValues(typeof(CategoryEnum))
				select c;
	query1.Dump("query 1");
	
	var query2 = new[] { CategoryEnum.Reminder, CategoryEnum.Problem, CategoryEnum.Finance }.Select(t => t);
	query2.Dump("query 2");	
	
	// Enumerable.Cast<TResult> Method
	// https://msdn.microsoft.com/library/bb341406(v=vs.100).aspx
	var query3 = Enum.GetValues(typeof(CategoryEnum)).Cast<CategoryEnum>().Select( t => t );
	query3.Dump("query 3");
	
	Enumerable.Range(0, 2).Select(x => (CategoryEnum)x).Dump("test");
}

// Define other methods and classes here
public enum CategoryEnum
{
   Reminder,
   Problem,
   Finance
}