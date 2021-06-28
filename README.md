# realm-schema-editor
A node editor for MongoDB Realm's rules and schemas, based on Unity's GraphView system.

## Disclaimer
This editor is code for a Unity project, and therefore requires a prior installation of Unity and
configuration of a Unity project. The code does not directly edit your MongoDB Realm rules and schemas.
This project is solely intended to help you visualize your schemas and rules, allowing you to:
 * create roles with insert, delete, read, write, and search permissions
 * create schemas with fields that contain a name and BSON type
 * connect fields from one schema to another
 * give certain roles permissions to certain schemas
 
## Example Configuration
![Image showing a node editor with owner and non-owner roles for users and messages collections](example.png)
