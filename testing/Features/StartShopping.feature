Feature: Start shopping
    Go the product page
    
    Scenario: Go to shop
        Given I am on the homepage
        When I click on Start Shopping
        Then I should see products
        
