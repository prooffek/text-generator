Let’s imagine despite having a lot of the ready-to-use packages and services for text generators by templates we need to have our own implementation. Of course it’s not a real task but there are some requirements:

Templates should contain plain text with placeholders which will be replaced with the real data from the data model.
Placeholder should contain the name (path) of the property from the model (also need to support nested properties like “Address.City”). The format of the placeholder is not determined so it can be any format. 
There is no need to support properties as collections (Lists, Arrays etc) but you can if you want.
Relation between template and type of data model should be 1-to-1 (each template should have only one related data type and vice versa).
There should be a simple way to generate text from a template using a data model (there is no restriction - it could be an extension method or service with a really simple public call or whatever but the real logic should be encapsulated).
Unit tests are not required but really nice to have.
UI is not required.


Example:

Template: 

“Hello {Name},


We will be glad to see you in our office in {Address.City} at {Address.Line1}.

Looking forward to meeting with you!


Best,
Our company.”

DataModel:
var dataModel = new DataModel
{ 
          Name: “John Doe”, 
          Address = new Address
          {
                City = “Budapest”,
                Line1= “Main Street, 1”
          }
};

Result:

“Hello John Doe,


We will be glad to see you in our office in Budapest at Main Street, 1.

Looking forward to meeting with you!


Best,
Our company.”