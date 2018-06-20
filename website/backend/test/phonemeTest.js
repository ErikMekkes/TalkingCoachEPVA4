const assert = require('chai').assert;
const httpMocks = require("node-mocks-http");
const EventEmitter =  require('events');
const exampleRouteHandler = require("../routes/api/v1/phoneme");

  describe("Test text to phoneme conversion.", () => {
    it("Hello world", function (done) {
      const mockRequest = httpMocks.createRequest({
        method: "GET",
        url: "/?text=Hello&lang=en-US",
        params: {
          text: "Hello",
          lang: 'en-US'
        }
      });
      const mockResponse = httpMocks.createResponse({eventEmitter: EventEmitter});
      exampleRouteHandler(mockRequest, mockResponse);
      
      const expectedResponseBody = [ 'HH', 'AH', 'L', 'OW' ];

      mockResponse.on('send', () => {
        assert.deepEqual(JSON.parse(mockResponse._getData()).phonemes, expectedResponseBody);
        done();
      });
    });

    it("Introduction sentence", function (done) {
        const mockRequest = httpMocks.createRequest({
          method: "GET",
          url: "/?text=Hello we are epva4&lang=en-US",
          params: {
            text: "Hello",
            lang: 'en-US'
          }
        });
        const mockResponse = httpMocks.createResponse({eventEmitter: EventEmitter});
        exampleRouteHandler(mockRequest, mockResponse);
        
        const expectedResponseBody = [ 'HH', 'AH', 'L', 'OW', '/', 'W', 'IY', '/', 'AA', 'R', 'R', '/', 'EH', 'P', 'V', 'AH', '/', 'F', 'AO' ];
  
        mockResponse.on('send', () => {
          assert.deepEqual(JSON.parse(mockResponse._getData()).phonemes, expectedResponseBody);
          done();
        });
      });

      it("Demo sentence", function (done) {
        const mockRequest = httpMocks.createRequest({
          method: "GET",
          url: "/?text=The quick brown fox jumps over the lazy dog&lang=en-US",
          params: {
            text: "Hello",
            lang: 'en-US'
          }
        });
        const mockResponse = httpMocks.createResponse({eventEmitter: EventEmitter});
        exampleRouteHandler(mockRequest, mockResponse);
        
        const expectedResponseBody = [ 'DH', 'AH', '/', 'K', 'W', 'IH', 'K', '/', 'B', 'R', 'AW', 'N', '/', 'F', 'AA', 'K', 'S', '/', 'JH', 'AH', 'M', 'P', 'S', '/', 'OW', 'V', 'ER', '/', 'DH', 'AH', '/', 'L', 'EY', 'Z', 'IY', '/', 'D', 'AA', 'G' ];
  
        mockResponse.on('send', () => {
          assert.deepEqual(JSON.parse(mockResponse._getData()).phonemes, expectedResponseBody);
          done();
        });
      });
  });