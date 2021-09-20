Feature: Validation
	In order to support various client app scenarios
	As an app developer
	I want to get notifications about model validity

Scenario: Valid test value object should have no errors
	When The simple test value object is created with name 'valid name'
	Then The simple test value object has no errors

Scenario: Invalid test value object should have errors
	When The simple test value object is created with name 'in$valid name'
	Then The simple test value object has errors

Scenario: Valid simple editable model should have no errors
	When The simple editable model is created with valid name
	Then The simple editable model has no errors

Scenario: Invalid simple editable model should have errors
	When The simple editable model is created with invalid name
	Then The simple editable model has errors

Scenario: Setting external error to a valid simple editable model should result in model with errors
	When The simple editable model is created with valid name
	And The simple editable model is updated with external error
	Then The simple editable model has errors

Scenario: Setting and clearing external error to a valid simple editable model should result in model with no errors
	When The simple editable model is created with valid name
	And The simple editable model is updated with external error
	And The simple editable model is cleared from external errors
	Then The simple editable model has no errors

Scenario: Setting property to invalid value to a valid simple editable model should result in model with errors
	When The simple editable model is created with valid name
	And The simple editable model is updated with invalid value for property
	Then The simple editable model has errors

Scenario: Setting property to invalid value to a valid simple editable model should raise error notification
	When The simple editable model is created with valid name
	And The simple editable model is updated with invalid value for property
	Then The error notification should be raised

Scenario: Valid simple model should have no errors
	When The simple model is created with valid name
	Then The simple model has no errors

Scenario: Invalid simple model should have errors
	When The simple model is created with invalid name
	Then The simple model has errors

Scenario: Setting external error to a valid simple model should result in model with errors
	When The simple model is created with valid name
	And The simple model is updated with external error
	Then The simple model has errors

Scenario: Setting and clearing external error to a valid simple model should result in model with no errors
	When The simple model is created with valid name
	And The simple model is updated with external error
	And The simple model is cleared from external errors
	Then The simple model has no errors

Scenario: Setting property to invalid value to a valid simple model should result in model with errors
	When The simple model is created with valid name
	And The simple model is updated with invalid value for property
	Then The simple model has errors

Scenario: Setting property to invalid value to a valid simple model should raise error notification
	When The simple model is created with valid name
	And The simple model is updated with invalid value for property
	Then The error notification should be raised

Scenario: Valid editable model should have no errors
	When The editable model is created with valid title
	Then The editable model has no errors

Scenario: Invalid editable model should have errors
	When The editable model is created with invalid title
	Then The editable model has errors

Scenario: Setting property to invalid value to a valid editable model should result in model with errors
	When The editable model is created with valid title
	And The editable model is updated with invalid value for property
	Then The editable model has errors

Scenario: Setting property to invalid value to a valid editable model should raise error notification
	When The editable model is created with valid title
	And The editable model is updated with invalid value for property
	Then The error notification should be raised

Scenario: Assigning internal property a valid value should result in model with no errors
	When The composite editable model is created
	And The internal model property is assigned a valid value
	Then The composite editable model has no errors

Scenario: Assigning internal property an invalid value should result in model with errors
	When The composite editable model is created
	And The internal model property is assigned an invalid value
	Then The composite editable model has errors

Scenario: Resetting internal model should raise error notification
	When The composite editable model is created
	And The internal model is reset
	Then The error notification should be raised

Scenario: Getting errors for a property without validation info should yield empty collection
	When The simple editable model is created with valid name
	Then The errors collection for property without validation info is empty

Scenario: Overriding error presentation should return overridden presentation for the model error
	When The simple editable model with overridden presentation is created
	And The simple editable model is updated with external error
	Then The simple editable model with presentation error should be 'overridden presentation'

#This feature isn't supported yet
@Ignore
Scenario: Assigning internal property an invalid value should raise error notification
	When The composite editable model is created
	And The internal model property is assigned an invalid value
	Then The error notification should be raised
