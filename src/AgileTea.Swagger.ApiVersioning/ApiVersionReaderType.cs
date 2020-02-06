namespace AgileTea.Swagger.ApiVersioning
{
    /// <summary>
    /// Maps to a corresponding Api Version Read - Url Segment, Header or Query Parameter
    /// </summary>
    public enum ApiVersionReaderType
    {
        /// <summary>
        /// Version is embedded with the url, i.e. api/V1/SomeOperation
        /// </summary>
        UrlSegment,

        /// <summary>
        /// Version is declared as a query parameter, i.e. api/SomeOperation?api-version=v1.0
        /// </summary>
        QueryParameter,

        /// <summary>
        /// Api version is declared within a header parameter, i.e. x-api-version
        /// </summary>
        Header
    }
}