Some helpful tools.
All contents of "Editor" folder are removed from the final build.

* **Line Ending**
Unifies line endings to prevent warnings (Windows, Linux, Unix).

* **Bulk Rename**
Rename multiple objects at once.

* **Readonly attribute**
Marks a field as read-only in the inspector. Use **[ReadonlyField]**.

* **Help attribute**
Allows adding info boxes to the inspector without making editor scripts.

> * [Help("This is some help text!", UnityEditor.MessageType.None)]
* [Help("This is some informatory text!", UnityEditor.MessageType.Info)]
* [Help("This is some warning text!", UnityEditor.MessageType.Warning)]
* [Help("This is some error text!", UnityEditor.MessageType.Error)]
