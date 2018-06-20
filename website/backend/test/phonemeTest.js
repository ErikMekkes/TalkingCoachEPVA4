const assert = require('chai').assert;
const httpMocks = require("node-mocks-http");
const EventEmitter =  require('events');
const exampleRouteHandler = require("../routes/api/v1/phoneme");

  describe("Example Router", () => {
    it("should return 'hello world' for GET /example", function (done) {
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
        console.log("data = " +  JSON.parse(mockResponse._getData()).phonemes);
        assert.deepEqual(JSON.parse(mockResponse._getData()).phonemes,expectedResponseBody);
        done();
      });
    });
  });