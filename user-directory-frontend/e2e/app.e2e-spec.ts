import { test, expect } from '@playwright/test';

test.describe('User Directory App', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/');
  });

  test('User list grid and search are visible', async ({ page }) => {
    await expect(page.locator('.user-list-wrapper')).toBeVisible();
    await expect(page.locator('input[placeholder="Search"]')).toBeVisible();
    await expect(page.locator('.user-grid')).toBeVisible();
  });

  test('Search icon triggers search', async ({ page }) => {
    const searchInput = page.locator('input[placeholder="Search"]');
    const searchIcon = page.locator('.fa-search');
    await searchInput.fill('test');
    await searchIcon.click();
    
    await expect(
      page.locator('.user-grid, div:has-text("No users found")')
    ).toBeVisible();
  });

  test('Can add a new user via the form', async ({ page }) => {
    await page.fill('input[name="firstName"]', 'E2E');
    await page.fill('input[name="lastName"]', 'TestUser');
    await page.fill('input[name="company"]', 'TestCo');
    await page.selectOption('select[name="sex"]', 'M');
    await page.fill('input[name="phone"]', '1234567890');
    await page.fill('input[name="address"]', '123 Test St');
    await page.fill('input[name="city"]', 'Testville');
    await page.fill('input[name="country"]', 'Testland');
    const roleSelect = page.locator('select[name="roleId"]');
    const roleOptions = await roleSelect.locator('option').all();
    if (roleOptions.length > 1) {
      await roleSelect.selectOption({ index: 1 });
    }

    const activeCheckbox = page.locator('input[name="active"]');
    if (!(await activeCheckbox.isChecked())) {
      await activeCheckbox.check();
    }

    await page.click('button[type="submit"]');

    await expect(page.locator('.toast-success, .user-grid')).toBeVisible({
      timeout: 3000,
    });

    while (
      (await page.locator('button:has-text("Next »"):not([disabled])').count()) > 0
    ) {
      await page.click('button:has-text("Next »")');
      await page.waitForTimeout(200);
    }

    await expect(page.locator('.user-grid')).toContainText('E2E TestUser');
  });
});
