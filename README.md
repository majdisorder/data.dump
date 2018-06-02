# Data.Dump
A C# data dump engine for easy creation of data extractions based on any dataset or poco, with a low memory footprint.
Currently Data.Dump only supports saving to SQL Server, but is easily extendable to support other platforms.

## What's it for?
The library grew out of the need to transperantly store large datasets, without knowing the datastructure beforehand. 
Here are some basic use cases, I'm sure you can come up with a few more.

* Dynamically storing data from various sources, be it a remote API, a CSV file, another database, or pretty much anything you can throw at it. 
* Creation of data extractions for use with BI tools
* Creation of tables for import procedures 
* Quick dump to the database of C# objects in order to examine structure and data
* Logging 
* ...

## How does it work?
Under the hood Data.Dump uses .NET's DataTables and DataSets for processing the data. While you can directly pass in your own DataTable or DataSet, the real power lies in the ability to pass in an any old object or collection thereof.

When you pass in C# objects, these will be translated to the appropriate datastructure. Simple types will automatically be resolved, however complex types will need some mapping. Depending on your configuration, nested objects or lists will automatically be translated to seperate tables in the database, with a foreign key in place. 

During processing, the data is transfered to a temptable using SQL Server's powerful bulkcopy technology. When ready, **any existing table with the same name will be dropped**, and replaced by the temptable. In this way the live table will always reflect the latest structure and data, while minimizing downtime. 

Table names are automatically resolved based on the Clr Type, but you can easily pass in your naming when necessary.

Special care has been taken to reduce the memory footprint while creating and storing datasets. Ofcourse, when you choose to pass in gigantic DataTables, you're on your own.

## How can I use it?
### Examples
Some examples and documentaion will be described here soon. 

For now, please refer to the included sample project.

### Instalation
A nuget package will be available shortly. 


*Please note this is a very early version of the product. Feel free to use and experiment with it, but be cautious when deploying in a production environment.*
