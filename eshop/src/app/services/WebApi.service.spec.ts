/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { WebApiService } from './WebApi.service';

describe('Service: WebApi', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WebApiService]
    });
  });

  it('should ...', inject([WebApiService], (service: WebApiService) => {
    expect(service).toBeTruthy();
  }));
});
