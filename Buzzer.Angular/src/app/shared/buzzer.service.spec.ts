import { TestBed, inject } from '@angular/core/testing';

import { BuzzerService } from './buzzer.service';

describe('BuzzerService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BuzzerService]
    });
  });

  it('should be created', inject([BuzzerService], (service: BuzzerService) => {
    expect(service).toBeTruthy();
  }));
});
