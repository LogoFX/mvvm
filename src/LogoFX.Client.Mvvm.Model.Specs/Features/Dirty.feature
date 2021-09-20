Feature: Dirty
	In order to support various client app scenarios
	As an app developer
	I want the framework to manage dirty object state properly

Scenario: Initially created simple editable model is not marked as dirty
	When The simple editable model is created with valid name
	Then The simple editable model is not marked as dirty

Scenario: Making simple editable model dirty results in model which is marked as dirty
	When The simple editable model is created with valid name
	And The simple editable model is made dirty
	Then The simple editable model is marked as dirty

Scenario: Setting property to invalid value to a valid simple editable model results in model which is marked as dirty
	When The simple editable model is created with valid name
	And The simple editable model is updated with invalid value for property
	Then The simple editable model is marked as dirty

Scenario: Initially created composite editable model is not marked as dirty
	When The composite editable model is created
	Then The composite editable model is not marked as dirty

Scenario: Setting inner property value to invalid value to a valid composite editable model results in model which is marked as dirty
	When The composite editable model is created
	And The composite editable model is updated with invalid value for inner property value
	Then The composite editable model is marked as dirty

@WI20
Scenario: Resetting internal model should raise dirty notification
	When The composite editable model is created
	And The internal model is reset
	Then The dirty notification should be raised

Scenario: Clearing dirty state for a composite editable model along with its children should result in model wich is not marked as dirty
	When The composite editable model is created
	And The composite editable model is updated with invalid value for inner property value
	And The composite editable model is cleared of dirty state along with its children
	Then The composite editable model is not marked as dirty

Scenario: Clearing dirty state for a composite editable model without its children should result in model wich is marked as dirty
	When The composite editable model is created
	And The composite editable model is updated with invalid value for inner property value
	And The composite editable model is cleared of dirty state without its children
	Then The composite editable model is marked as dirty

Scenario: Setting inner property value of a child collection item 
		  to invalid value to a valid composite editable model 
		  results in model which is marked as dirty
	When The composite editable model with collection is created
	And The composite editable model is updated with invalid value for child collection item inner property value
	Then The composite editable model is marked as dirty

Scenario: Removing item from child collection of a valid composite editable model results in model which is marked as dirty
	When The composite editable model with collection is created
	And The composite editable model is updated by removing child item from the collection
	Then The composite editable model is marked as dirty

Scenario: Removing item from child collection of a valid composite editable model then clearing its dirty status and setting child property value results in model which is not marked as dirty
	When The composite editable model with collection is created
	And The composite editable model is updated by removing child item from the collection
	And The composite editable model is cleared of dirty state along with its children
	And The child item is assigned an invalid property value
	Then The composite editable model is not marked as dirty

Scenario: Removing item from child collection of a valid explicit composite editable model results in model which is marked as dirty
	When The explicit composite editable model with collection is created
	And The explicit composite editable model is updated by removing child item from the collection
	Then The explicit composite editable model is marked as dirty

Scenario: Removing item from child collection of a valid explicit composite editable model then clearing its dirty status and setting child property value results in model which is not marked as dirty
	When The explicit composite editable model with collection is created
	And The explicit composite editable model is updated by removing child item from the collection
	And The explicit composite editable model is cleared of dirty state along with its children
	And The child item is assigned an invalid property value
	Then The explicit composite editable model is not marked as dirty

Scenario: Removing item from child collection of a valid composite editable model should raise dirty notification
	When The composite editable model with collection is created
	And The composite editable model is updated by removing child item from the collection
	Then The dirty notification should be raised

Scenario: Setting property with before value update logic invokes it in the right order
	When The editable model with before value update logic is created
	And The editable model with before value update logic is made dirty
	Then The before value update logic is invoked before model is made dirty

#This feature isn't supported yet
@Ignore
Scenario: Updating self-referencing model should result in model which is marked as dirty
	When The self-referencing model is created
	And The self-referencing model is assigned itself
	Then The self-referencing model is marked as dirty


