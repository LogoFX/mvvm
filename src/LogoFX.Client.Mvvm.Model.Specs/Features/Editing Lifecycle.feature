Feature: Editing Lifecycle
	In order to develop rich functional apps faster
	As an app developer
	I want the framework to support standard editing lifecycle

Scenario: Setting property to invalid value to a valid editable model and then cancelling the changes should result in model with no errors
	When The editable model is created with valid title
	And The editable model is updated with invalid value for property
	And The editable model changes are cancelled
	Then The editable model has no errors

Scenario: Setting external error to a valid editable model after manual property update and then cancelling the changes should result in model with no errors
	When The editable model is created with valid title
	And The editable model is updated with value 'Mr.' for property
	And The editable model is updated with external error
	And The editable model changes are cancelled
	Then The editable model has no errors

Scenario: Setting external error to a valid editable model before manual property update and cancelling the changes after the manual property update should result in model with errors
	When The editable model is created with valid title
	And The editable model is updated with external error
	And The editable model is updated with value 'Mr.' for property
	And The editable model is cleared from external errors
	And The editable model changes are cancelled
	Then The editable model has errors

Scenario: Cancelling changes to a valid editable model with read only field should result in model which has no errors
	When The editable model with read only field is created
	And The editable model with read only field is updated with new status
	And The editable model with read only field changes are cancelled
	Then The editable model with read only field has no errors

Scenario: Cancelling changes to a valid editable model should restore the original state and resulting model which is not marked as dirty
	When The editable model with undo redo and valid name is created
	And The name is updated to 'NameOne'
	And The editable model with undo redo changes are cancelled
	Then The name should be identical to the valid name
	And The editable model with undo redo has no errors

Scenario: Cancelling changes to a deep hierarchy model should restore its grandchildren state
	When The deep hierarchy model with all generations is created
	And The grandchild name is updated to 'New Name'
	And The deep hierarchy model changes are cancelled
	Then The deep hierarchy model is not marked as dirty
	And The grandchild name should be identical to the valid name

Scenario: Cancelling changes after one editing operation restores the model to initial state and marks it as not dirty
	When The composite editable model with undo-redo is created with initial data
	And The collection of items is updated with new value 647
	And The composite editable model with undo-redo changes are cancelled
	Then The collection of items should be equivalent to the initial data
	And The composite editable model with undo-redo is not marked as dirty

Scenario: Cancelling changes after a committed change restores the model to the last committed state and marks it as not dirty
	When The composite editable model with undo-redo is created with initial data
	And The collection of items is updated with new value 647
	And The composite editable model with undo-redo changes are committed
	And The collection of items is updated with new value 555
	And The composite editable model with undo-redo changes are cancelled
	Then The collection of items should be equivalent to the initial data with the new value
	And The composite editable model with undo-redo is not marked as dirty

Scenario: Changing child inside a deep hierarchy model should enable changes cancellation
	When The deep hierarchy model with all generations is created
	And The child is updated with removing the grandchild
	Then The deep hierarchy model changes can be cancelled

Scenario: Committing changes should save changes and result in model which is not marked as dirty
	When The child model is created
	And The grandchild model is created
	And The grandchild is added to the child
	And The deep hierarchy model is created
	And The child is added to the deep hierarchy model
	And The child is updated with removing the grandchild
	And The deep hierarchy model changes are committed
	Then The deep hierarchy model changes can not be cancelled
	And The deep hierarchy model is not marked as dirty
	And The deep hierarchy model contains the child
	And The child does not contain the grandchild

Scenario: Cancelling changes should cancel all changes and result in model which is not marked as dirty
	When The child model is created
	And The grandchild model is created
	And The grandchild is added to the child
	And The deep hierarchy model is created
	And The child is added to the deep hierarchy model
	And The child is updated with removing the grandchild
	And The deep hierarchy model changes are cancelled
	Then The deep hierarchy model changes can not be cancelled
	And The deep hierarchy model is not marked as dirty
	And The deep hierarchy model contains the child
	And The child contains the grandchild

Scenario: Advanced cancelling changes scenario should cancel all changes and result in model which is not marked as dirty
	When The first child model is created
	And The second child model is created
	And The first grandchild model is created
	And The second grandchild model is created
	And The first grandchild is added to the first child
	And The second grandchild is added to the first child
	And The deep hierarchy model is created
	And The first child is added to the deep hierarchy model
	And The second child is added to the deep hierarchy model
	And The first child is removed from the deep hierarchy model
	And The deep hierarchy model changes are cancelled
	And The first child is updated with removing the first grandchild
	And The deep hierarchy model changes are cancelled
	Then The deep hierarchy model changes can not be cancelled
	And The deep hierarchy model is not marked as dirty
	And The deep hierarchy model contains all children
	And The first child contains all grandchildren

#This feature isn't supported yet
@Ignore
Scenario: Cancelling changes for self-referencing model should result in model which is not marked as dirty
	When The self-referencing model is created
	And The self-referencing model is assigned itself
	And The self-referencing model changes are cancelled
	Then The self-referencing model is not marked as dirty

