import { NgFirebaseBuzzerPage } from './app.po';

describe('ng-firebase-buzzer App', () => {
  let page: NgFirebaseBuzzerPage;

  beforeEach(() => {
    page = new NgFirebaseBuzzerPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!!');
  });
});
